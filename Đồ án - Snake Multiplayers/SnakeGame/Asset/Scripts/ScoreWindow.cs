using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScoreWindow : MonoBehaviour
{
    [SerializeField] private List<PlayerRankWindow> playerRankWindows;

    private Dictionary<string, int> ranks = new Dictionary<string, int>();

    public void UpdateRankInfo(string username, string rank, string score)
    {
        int rankInt = int.Parse(rank);
        if(rankInt >= 6)
        {
            return;
        }
        if(ranks.ContainsKey(username))
        {
            ranks[username] = rankInt;
        }
        else
        {
            ranks.Add(username, rankInt);
        }
        playerRankWindows[rankInt - 1].SetPlayer(username, rank, score);
    }

    public void DestroyPlayerRank(string username)
    {
        int rank = ranks[username];
        if(rank >= 6)
        {
            return;
        }
        playerRankWindows[rank - 1].Disable();
        int ranksSize = ranks.Values.Count;
        if(ranksSize > rank)
        {
            for (int i = rank - 1; i <= ranksSize - 1; i++)
            {
                int nextIndex = i + 1;
                playerRankWindows[i].SetPlayer(playerRankWindows[nextIndex].GetUsername(), nextIndex.ToString(), playerRankWindows[nextIndex].GetScore());
                if(nextIndex == ranksSize - 1)
                {
                    playerRankWindows[nextIndex].Disable();
                    break;
                }
            }
        }
    }
}
