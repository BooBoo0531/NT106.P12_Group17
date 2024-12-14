using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListRankItemManager : MonoBehaviour
{

    public static ListRankItemManager Instance { 
        get
        {
            return instance;
        }
    }
    private static ListRankItemManager instance;
    [SerializeField] private GameObject rankItemPrelab;
    [SerializeField] private GameObject contentsView;
    private Dictionary<string, RankItem> rankItems = new Dictionary<string, RankItem>();

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        } else
        {
            Destroy(gameObject);
        }
        gameObject.SetActive(false);
    }

    public void UpdateRankInfo(string username, string rank, string score, string round)
    {
        if(rankItems.Count > 0 && rankItems.ContainsKey(username))
        {
            rankItems[username].SetPlayer(username, rank, score, round);
        }
        else
        {
            GameObject rankItem = Instantiate(rankItemPrelab, contentsView.transform);
            RankItem rankItemComponent = rankItem.GetComponent<RankItem>();
            rankItemComponent.SetPlayer(username, rank, score, round);
            rankItems.Add(username, rankItemComponent);
        }
    }

    public void DestroyPlayerRank(string username)
    {
        if(rankItems.Count == 0 || !rankItems.ContainsKey(username))
        {
            return;
        }
        Destroy(rankItems[username].gameObject);
        rankItems.Remove(username);
    }

    public void Clear()
    {
        if(rankItems.Count == 0)
        {
            return;
        }
        foreach (var item in rankItems)
        {
            Destroy(item.Value.gameObject);
        }
        rankItems.Clear();
    }
}
