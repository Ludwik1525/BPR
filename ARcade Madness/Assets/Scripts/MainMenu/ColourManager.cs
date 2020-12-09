using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class ColourManager : MonoBehaviour
{
    public Color32[] coloursArray;

    private PhotonView PV;
    
    private Transform playersContainer;

    //a method for setting a new colour for the player who calls it
    [PunRPC]
    void RPC_ApplyColour(int colourNo, string myName)
    {
        // get the names displayer
        playersContainer = GameObject.Find("PlayerDisplayer").transform;

        for (int i = playersContainer.childCount - 1; i >= 0; i--) // for each player in the displayer
            {
                if (playersContainer.GetChild(i).GetChild(0).GetComponent<Text>().text == myName) // if it is me
                {
                    playersContainer.GetChild(i).GetChild(1).GetComponent<Image>().color = coloursArray[colourNo];
                    // update the colour
                }
            }
    }
}
