using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSetup : MonoBehaviour
{
    public static GameSetup GS;

    private void OnEnable() {
        if (GameSetup.GS == null) {
            GameSetup.GS = this;
        }
    }
}
