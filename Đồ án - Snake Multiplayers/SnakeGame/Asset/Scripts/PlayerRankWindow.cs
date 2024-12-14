using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class PlayerRankWindow : MonoBehaviour
    {
        [SerializeField] private Text rankText;
        [SerializeField] private Text scoreText;
        [SerializeField] private Text nameText;
        [SerializeField] private Image image;

        public string GetUsername()
        {
            return nameText.text;
        }
        public string GetRank()
        {
            return rankText.text;
        }

        public string GetScore()
        {
            return scoreText.text;
        }
        private void Start()
        {
            rankText.enabled = false;
            scoreText.enabled = false;
            nameText.enabled = false;
            image.enabled = false;
        }

        public void SetPlayer(string username, string rank, string score)
        {
            if(rankText != null)
            {
                rankText.text = rank;
            }
            if(scoreText != null)
            {
                scoreText.text = score;
            }
            if(nameText != null)
            {
                nameText.text = username;
            }
            rankText.enabled = true;
            scoreText.enabled = true;
            nameText.enabled = true;
            image.enabled = true;
        }

        public void Disable()
        {
            rankText.enabled = false;
            scoreText.enabled = false;
            nameText.enabled = false;
            image.enabled = false;
        }
    }
}
