using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Stats")]
    public float speed = 7f;

    public Rigidbody rb;
    public Inventory inventory;
    public TMP_Text coinsLabel;
    public TMP_Text itemTukiLabel;
    public TMP_Text itemIagraLabel;
    public TMP_Text itemClonaLabel;
    private int costOfTuki = 10;
    private float costOfIagra = 20f;
    private float costOfClona = 30f;

    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    void Start()
    {
        inventory.Coin = 100f;
        coinsLabel.text = inventory.Coin.ToString();
        inventory.tuki = 0f;
        inventory.iagra = 0f;
        inventory.clona = 0f;
    }

    void Update()
    {
        Movement();
    }

    void Movement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        rb.velocity = new Vector3(x, 0, z) * speed;
    }

    #region Shop
    public void buyTuki()
    {
        if (inventory.Coin >= costOfTuki)
        {

            inventory.Coin -= costOfTuki;
            coinsLabel.text = inventory.Coin.ToString();
            inventory.AddTuki();
            PrintItemQuantityTuki();

        }
        else
        {
            print("Coins Insuficientes");
        }
    }

    public void buyIagra()
    {
        if (inventory.Coin >= costOfIagra)
        {

            inventory.Coin -= costOfIagra;
            coinsLabel.text = inventory.Coin.ToString();
            inventory.AddIagra();
            PrintItemQuantityIagra();
        }
        else
        {
            print("Coins Insuficientes");
        }
    }

    public void buyClona()
    {
        if (inventory.Coin >= costOfClona)
        {

            inventory.Coin -= costOfClona;
            coinsLabel.text = inventory.Coin.ToString();
            inventory.AddClona();
            PrintItemQuantityClona();
        }
        else
        {
            print("Coins Insuficientes");
        }
    }

    public void PrintCoinsQuantity()
    {
        coinsLabel.text = inventory.Coin.ToString();
    }
    public void PrintItemQuantityTuki()
    {
        itemTukiLabel.text = inventory.tuki.ToString();
    }
    public void PrintItemQuantityIagra()
    {
        itemIagraLabel.text = inventory.iagra.ToString();
    }
    public void PrintItemQuantityClona()
    {
        itemClonaLabel.text = inventory.clona.ToString();
    }
    #endregion
}
