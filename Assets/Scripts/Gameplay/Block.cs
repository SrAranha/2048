using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Block : MonoBehaviour
{
    public int currentVariation = 0;
    public List<BlockSetups> setups; 
    [SerializeField] private Image _image;
    [SerializeField] private TMP_Text _numberText;

    private void OnValidate()
    {
        _image = GetComponent<Image>();
        _numberText = GetComponentInChildren<TMP_Text>();
    }
    // Start is called before the first frame update
    void Start()
    {
        ChangeBlockVariant(true);
    }

    public void MergeBlocks(Block other)
    {
        Destroy(other.gameObject);
        ChangeBlockVariant(false);
    }
    private void ChangeBlockVariant(bool defaultVariation)
    {
        if (defaultVariation)
        {
            _image.color = setups[0].color;
            _numberText.text = setups[0].number.ToString();
        }
        else
        {
            currentVariation++;
            _image.color = setups[currentVariation].color;
            _numberText.text = setups[currentVariation].number.ToString();
        }
    }
}
[System.Serializable]
public class BlockSetups
{
    public int number;
    public Color color;
}
