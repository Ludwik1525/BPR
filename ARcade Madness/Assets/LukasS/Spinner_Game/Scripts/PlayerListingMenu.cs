using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerListingMenu : MonoBehaviour
{

    [SerializeField]
    private Transform _content;
    [SerializeField]
    private PlayerListing _playerListing;

    private List<PlayerListing> _listings = new List<PlayerListing>();

    // Start is called before the first frame update
    void Start()
    {

    }

    public void ReadyBtn()
    {

    }

    public void Ready()
    {
        PlayerListing listing = Instantiate(_playerListing, _content);
        if (listing != null)
        {
            listing.SetPlayerInfo(PhotonNetwork.LocalPlayer);
            _listings.Add(listing);
        }
    }
}
