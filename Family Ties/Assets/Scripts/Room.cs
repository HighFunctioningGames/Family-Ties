using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] GameObject roomTrigger;
    [SerializeField] GameObject fog;
    Material fogMaterial;
    [SerializeField] GameObject[] Items;


    [SerializeField] float duration;
    [SerializeField] float delayTime;

    public float playerCount = 0;
    public bool roomRevealed = false;
    public bool delayActive;
    public bool isFading;
    IEnumerator myDelay;
    IEnumerator myFadeOut;
    IEnumerator myFadeIn;

    void Start()
    {
        fogMaterial = fog.GetComponent<MeshRenderer>().material;
        delayActive = false;
        isFading = false;
    }

    public void RoomEnter()
    {
        if (playerCount == 0)
        {
            if (delayActive)
            {
                myDelay = delay();
                StopCoroutine(myDelay);
                delayActive = false;
            }
            else if(isFading)
            {
                roomRevealed = true;
                StopCoroutine(myFadeIn);
                myFadeOut = fadeOut();
                StartCoroutine(myFadeOut);
            }
            else
            {
                if (!roomRevealed)
                {
                    roomRevealed = true;
                    myFadeOut = fadeOut();
                    StartCoroutine(myFadeOut);
                }
            }
        }
        playerCount++;
    }

   public void RoomExit()
    {
        playerCount--;
        if (playerCount == 0)
        {
            if (roomRevealed)
            {
                myDelay = delay();
                StartCoroutine(myDelay);
            }
        }
    }

    void ItemSwitch(bool state)
    {
        for (int i = 0; i < Items.Length; i++)
        {
            if (Items[i] != null)
            {
                Items[i].gameObject.SetActive(state);
            }
        }
    }

    IEnumerator fadeOut()
    {
        isFading = true;
        ItemSwitch(true);
        float counter = 0;
        float start = fogMaterial.color.a;
        Color fogColor = fogMaterial.color;
        while (counter < duration)
        {
            counter += Time.deltaTime;

            float alpha = Mathf.Lerp(start, 0, counter / duration);

            fogMaterial.color = new Color(fogColor.r, fogColor.g, fogColor.b, alpha);

            yield return null;
        }
        isFading = false;
    }

    IEnumerator fadeIn()
    {
        roomRevealed = false;
        isFading = true;
        delayActive = false;
        ItemSwitch(false);
        float start = fogMaterial.color.a;
        float counter = 0;
        Color fogColor = fogMaterial.color;
        while (counter < duration)
        {
            counter += Time.deltaTime;

            float alpha = Mathf.Lerp(start, 0.4f, counter / duration);

            fogMaterial.color = new Color(fogColor.r, fogColor.g, fogColor.b, alpha);

            yield return null;
        }
        isFading = false;
    }

    IEnumerator delay()
    {
        delayActive = true;
        yield return new WaitForSeconds(delayTime);
        myFadeIn = fadeIn();
        StartCoroutine(myFadeIn);
    }

}
