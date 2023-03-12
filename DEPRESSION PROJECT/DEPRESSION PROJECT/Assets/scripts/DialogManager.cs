using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using System.Linq;

public class DialogManager : MonoBehaviour
{
    public string[] sentences;

    public TMP_Text DialogText;

    public Animator anim;
    public TextAsset textFile;
    private bool isDisplayingSentence = false;

    // Start is called before the first frame update
    void Start()
    {
        // Load the text file as a TextAsset object
        

        // Split the contents of the file into an array of strings
        sentences = textFile.text.Split('\n');

        for (int i = 0; i < sentences.Length; i++)
        {
            sentences[i] = sentences[i].Trim();
        }
    }

    private bool isDialogOpen = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isDialogOpen)
            {
                if (isDisplayingSentence)
                {
                    StopAllCoroutines();
                    DialogText.text = "";
                    isDisplayingSentence = false;
                }
                else
                {
                    EndDialog();
                    isDialogOpen = false;
                }
            }
            else
            {
                if (sentences.Length > 0)
                {
                    isDialogOpen = true;
                    anim.SetBool("IsOpen", true);
                    DisplayNextSentence();
                }
            }
        }
    }

    public void StartDialog(Dialog dialog)
    {
        anim.SetBool("IsOpen", true);
        sentences = dialog.sentences;
    }

    public void DisplayNextSentence()
    {
        if (sentences.Length == 0)
        {
            // If we've displayed all the sentences, reset the array to its original state
            sentences = textFile.text.Split('\n');
            for (int i = 0; i < sentences.Length; i++)
            {
                sentences[i] = sentences[i].Trim();
            }
        }

        int randomIndex = UnityEngine.Random.Range(0, sentences.Length);
        string sentence = sentences[randomIndex];
        StartCoroutine(TypeSentence(sentence));

        // Add the sentence back to the end of the array
        string[] newSentences = new string[sentences.Length + 1];
        sentences.CopyTo(newSentences, 0);
        newSentences[newSentences.Length - 1] = sentence;
        sentences = newSentences;
    }


    IEnumerator TypeSentence(string sentence)
    {
        isDisplayingSentence = true;
        DialogText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            DialogText.text += letter;
            yield return new WaitForSeconds(0.02f);
        }
        isDisplayingSentence = false;
    }

    public void EndDialog()
    {
        Debug.Log("close");
        anim.SetBool("IsOpen", false);
        Debug.Log(anim.GetBool("IsOpen"));
    }
}