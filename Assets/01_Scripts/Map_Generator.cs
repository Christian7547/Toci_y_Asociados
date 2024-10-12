using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public class Cell
    {
        public bool visited = false;
        public bool[] status = new bool[4];  // 4 direcciones: arriba, derecha, abajo, izquierda
    }

    [System.Serializable]
    public class Rule
    {
        public GameObject room;
        public Vector2Int minPosition;
        public Vector2Int maxPosition;

        public bool hasSpawned = false; // Solo se debe spawnear una vez
        public bool obligatory; // Si es obligatorio generar esta sala
        public bool isSpawnRoom = false; // Indica si es la habitaci?n de Spawn
        public bool isBossRoom = false; // Indica si es la habitaci?n del Jefe

        public int ProbabilityOfSpawning(int x, int y)
        {
            if (x >= minPosition.x && x <= maxPosition.x && y >= minPosition.y && y <= maxPosition.y)
            {
                // Si es obligatorio y no ha sido generado a?n, da prioridad (2)
                if (obligatory && !hasSpawned)
                {
                    return 2;
                }
                // Si no es obligatorio, pero est? dentro del rango permitido
                return 1;
            }
            return 0;
        }
    }

    public Vector2Int size;
    public int startPos = 0;
    public Rule[] rooms; // Reglas para las habitaciones
    public Vector2 offset;

    private List<Cell> board;
    public GameObject player; // El jugador que debe spawnear en la habitaci?n de Spawn

    void Start()
    {
        MazeGenerator();
        GenerateDungeon();
    }

    void GenerateDungeon()
    {
        Vector2Int bossRoomPosition = Vector2Int.zero;
        bool spawnRoomPlaced = false;

        // Lista de posiciones para generar habitaciones normales, dejando BossRoom para el final
        List<Vector2Int> normalRoomPositions = new List<Vector2Int>();

        // Generar las habitaciones normales y la SpawnRoom primero
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                Cell currentCell = board[i + j * size.x];
                if (currentCell.visited)
                {
                    // Si no hemos colocado la SpawnRoom, la colocamos primero
                    if (!spawnRoomPlaced)
                    {
                        int spawnRoomIndex = GetRoomByTag("Respawn");
                        rooms[spawnRoomIndex].hasSpawned = true;

                        var spawnRoom = Instantiate(
                            rooms[spawnRoomIndex].room,
                            new Vector3(i * offset.x, 0, -j * offset.y),
                            Quaternion.identity,
                            transform
                        ).GetComponent<RoomBehaviour>();

                        // Solo actualizamos las tres direcciones (arriba, derecha, izquierda)
                        bool[] status = new bool[3] { currentCell.status[0], currentCell.status[1], currentCell.status[3] };
                        spawnRoom.UpdateRoom(status);
                        spawnRoom.name += " " + i + "-" + j;

                        spawnRoomPlaced = true;
                    }
                    else
                    {
                        // Si estamos en la última celda de la matriz, la reservamos para la BossRoom
                        if (i == size.x - 1 && j == size.y - 1)
                        {
                            bossRoomPosition = new Vector2Int(i, j);
                        }
                        else
                        {
                            // Guardamos la posición para habitaciones normales
                            normalRoomPositions.Add(new Vector2Int(i, j));
                        }
                    }
                }
            }
        }

        // Generamos todas las habitaciones normales
        foreach (Vector2Int position in normalRoomPositions)
        {
            int selectedRoom = GetRandomRoom(position.x, position.y);

            rooms[selectedRoom].hasSpawned = true;

            var newRoom = Instantiate(
                rooms[selectedRoom].room,
                new Vector3(position.x * offset.x, 0, -position.y * offset.y),
                Quaternion.identity,
                transform
            ).GetComponent<RoomBehaviour>();

            // Solo activamos las tres direcciones (arriba, derecha, izquierda)
            bool[] status = new bool[3] { board[position.x + position.y * size.x].status[0], board[position.x + position.y * size.x].status[1], board[position.x + position.y * size.x].status[3] };
            newRoom.UpdateRoom(status);
            newRoom.name += " " + position.x + "-" + position.y;
        }

        // Finalmente generamos la BossRoom en la última celda reservada
        if (bossRoomPosition != Vector2Int.zero)
        {
            int bossRoomIndex = GetRoomByTag("BossRoom");
            rooms[bossRoomIndex].hasSpawned = true;

            var bossRoom = Instantiate(
                rooms[bossRoomIndex].room,
                new Vector3(bossRoomPosition.x * offset.x, 0, -bossRoomPosition.y * offset.y),
                Quaternion.identity,
                transform
            ).GetComponent<RoomBehaviour>();

            // Solo activamos las tres direcciones (arriba, derecha, izquierda)
            bool[] status = new bool[3] { board[bossRoomPosition.x + bossRoomPosition.y * size.x].status[0], board[bossRoomPosition.x + bossRoomPosition.y * size.x].status[1], board[bossRoomPosition.x + bossRoomPosition.y * size.x].status[3] };
            bossRoom.UpdateRoom(status);
            bossRoom.name += " " + bossRoomPosition.x + "-" + bossRoomPosition.y;
        }
    }

    int GetRandomRoom(int i, int j)
    {
        List<int> availableRooms = new List<int>();

        for (int k = 0; k < rooms.Length; k++)
        {
            // Ignorar BossRoom y SpawnRoom para habitaciones normales
            if (rooms[k].room.CompareTag("Respawn") || rooms[k].room.CompareTag("BossRoom"))
            {
                continue;
            }

            // Si no es obligatoria pero est? dentro del rango permitido
            int spawnProbability = rooms[k].ProbabilityOfSpawning(i, j);
            if (spawnProbability == 1 && !rooms[k].hasSpawned)
            {
                availableRooms.Add(k);
            }
        }

        if (availableRooms.Count > 0)
        {
            return availableRooms[Random.Range(0, availableRooms.Count)];
        }

        return 0; // Fallback por defecto
    }



    void MazeGenerator()
    {
        board = new List<Cell>();

        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                board.Add(new Cell());
            }
        }

        int currentCell = startPos;
        Stack<int> path = new Stack<int>();
        int iterations = 0;

        while (iterations < 1000)
        {
            iterations++;
            board[currentCell].visited = true;

            if (currentCell == board.Count - 1) break;

            List<int> neighbors = CheckNeighbors(currentCell);

            if (neighbors.Count == 0)
            {
                if (path.Count == 0) break;
                currentCell = path.Pop();
            }
            else
            {
                path.Push(currentCell);
                int newCell = neighbors[Random.Range(0, neighbors.Count)];
                ConnectCells(currentCell, newCell);
                currentCell = newCell;
            }
        }
    }

    void ConnectCells(int currentCell, int newCell)
    {
        if (newCell > currentCell)
        {

            if (newCell - 1 == currentCell)
            {
                board[currentCell].status[2] = true;
                board[newCell].status[3] = true;
            }
            else
            {
                board[currentCell].status[1] = true;
                board[newCell].status[0] = true;
            }
        }
        else
        {

            if (newCell + 1 == currentCell)
            {
                board[currentCell].status[3] = true;
                board[newCell].status[2] = true;
            }
            else
            {
                board[currentCell].status[0] = true;
                board[newCell].status[1] = true;
            }
        }
    }

    List<int> CheckNeighbors(int cell)
    {
        List<int> neighbors = new List<int>();

        if (cell - size.x >= 0 && !board[cell - size.x].visited)
        {
            neighbors.Add(cell - size.x);
        }

        if (cell + size.x < board.Count && !board[cell + size.x].visited)
        {
            neighbors.Add(cell + size.x);
        }

        if ((cell + 1) % size.x != 0 && !board[cell + 1].visited)
        {
            neighbors.Add(cell + 1);
        }

        if (cell % size.x != 0 && !board[cell - 1].visited)
        {
            neighbors.Add(cell - 1);
        }

        return neighbors;
    }


    int GetRoomByTag(string tag)
    {
        for (int k = 0; k < rooms.Length; k++)
        {
            if (rooms[k].room.CompareTag(tag) && !rooms[k].hasSpawned)
            {
                return k;
            }
        }

        return 0; // Fallback por si no encuentra la habitaci?n (no deber?a suceder)
    }
}

