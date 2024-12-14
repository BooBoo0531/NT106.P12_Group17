using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankItem : MonoBehaviour
{
    [SerializeField] private Text rankText;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text nameText;
    [SerializeField] private Text roundText;

    public void SetPlayer(string username, string rank, string score, string round)
    {
        nameText.text = username;
        rankText.text = rank;
        scoreText.text = score;
        roundText.text = round + " Round";
    }
}
