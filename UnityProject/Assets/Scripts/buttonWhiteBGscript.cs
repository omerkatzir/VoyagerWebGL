using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class buttonWhiteBGscript : MonoBehaviour
{

    void startAnim()
    {
        gameObject.GetComponent<Animator>().enabled = false;//.SetInteger("state", 2);
        var tlbs = transform.parent.GetComponent<trackListButtonScript>();
        tlbs.StartAnimTimer();
    }

    public void endAnim()
    {
        var anim = gameObject.GetComponent<Animator>();
        anim.SetInteger("state", 3);
        anim.enabled = true;
    }

    void makeTextBlack()
    {
        transform.parent.GetChild(2).GetComponent<Text>().color = Color.black;
    }

    void makeTextWhite()
    {
        transform.parent.GetChild(2).GetComponent<Text>().color = Color.white;
    }

    void stopAnim()
    {
        gameObject.GetComponent<Animator>().enabled = false;
    }
}
