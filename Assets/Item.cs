using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public string ActivationKey;
    public Sprite DisplayImage;
    public int WoodNumber;
    public int RockNumber;
    public int CrystalNumber;
    
    [SerializeField] private TextMeshProUGUI activationKeyText;
    [SerializeField] private Image sprite;
    [SerializeField] private TextMeshProUGUI woodPriceText;
    [SerializeField] private TextMeshProUGUI rockPriceText;
    [SerializeField] private TextMeshProUGUI crystalPriceText;

}
