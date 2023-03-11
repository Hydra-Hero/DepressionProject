using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTRIGGER : MonoBehaviour
{
    public Dialog dialog;

    public void triggerDialog()
    {
        FindObjectOfType<DialogManager>().StartDialog(dialog);
    }
}
