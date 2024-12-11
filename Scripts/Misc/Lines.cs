using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Lines : MonoBehaviour
{
    [Header("Properties")]
    public TextMeshProUGUI textComponent;
    public string[] line;
    public float textSpeed;

    private int index;

    void Start()
    {
        StartDialogue();
    }

    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLines());
    }

    IEnumerator TypeLines()
    {
        foreach (char c in line[index].ToCharArray())
        {
            textComponent.text += c;

            yield return new WaitForSeconds(textSpeed);
        }
    }
}
