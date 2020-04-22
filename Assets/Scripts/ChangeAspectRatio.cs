using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeAspectRatio : MonoBehaviour
{
    public GameObject videoHolder;
    public Text btnText; 

    [System.Serializable]
    public class AspectRatios {
        public int w;
        public int h;

        public string PrintRatio() {
            return this.w.ToString() + " X " + this.h.ToString();
        }
    }

    public AspectRatios[] ratios;
    static int ctr = 0;

    public void ChangeRatio() {

        ctr = ctr + 1;
        ctr %= ratios.Length;

        float x =(float) ratios[ctr].h / (float)ratios[ctr].w;

        videoHolder.transform.localScale = new Vector3(1f,x, 1f);
        btnText.text = ratios[ctr].PrintRatio();
    }

}

//21X9   -  0.429
//16X9   -  0.5625
//4X3    -  0.75

