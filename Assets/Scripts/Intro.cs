using System.Collections;
using System.Collections.Generic;
using System;
using static GPGSManager.GPGSManager;
using UnityEngine;
using System.Text;
using UnityEngine.SceneManagement;
public class Intro : MonoBehaviour {

    public GameObject save;
    public void goToMain () {
        Initialize (false);
        Auth ((success) => {
            if (success) {
                ReadSaveData ("Save", (status, metadata) => {
                    if (status == GooglePlayGames.BasicApi.SavedGame.SavedGameRequestStatus.Success) {
                        string data = Encoding.UTF8.GetString (metadata, 0, metadata.Length - 1);
                        PlayerPrefs.SetString ("jaSav", data);
                    } else {
                        
                    }
                });
            }
            SceneManager.LoadScene (1);
        });
    }

}