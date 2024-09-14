using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Thirdweb;
using UnityEngine.UI;

public class BlockchainShopManager : MonoBehaviour
{
    private static BlockchainShopManager _instance;

    public static BlockchainShopManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<BlockchainShopManager>();

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("CanvasBlockchain");
                    _instance = singletonObject.AddComponent<BlockchainShopManager>();
                }

                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public int gold;
    public int hp;
    public int goldx2;

    private void Start()
    {
        gold = 0;
        hp = 0;
        goldx2 = 1;
    }

}
