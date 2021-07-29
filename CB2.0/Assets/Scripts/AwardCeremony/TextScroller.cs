using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum ConversationType
{
    simpleText = 0,
    simpleImage = 1,
    textAndImage = 2,
    playerInfo = 3, // Display player ID and player avatar
    playerMasks = 4 // Display the number of masks the player collected
}

[System.Serializable]
public class ConversationObject
{
    public string text;

    public Color textColor;

    public Sprite image;

    public ConversationType conversationType;

    public float delayInSeconds; // show for n seconds
}

public class TextScroller : MonoBehaviour
{
    public GameEvent onPlayNextMiniGame;

    public List<ConversationObject> openingConversations;

    public List<ConversationObject> closingConversations;

    public TMP_Text simpleText;

    public Image simpleImage;

    public TMP_Text compositeText;

    public Image compositeImage;

    public Color prizeCeremonyTextColor;

    public Sprite maskSprite;

    public Players players;

    private List<CeremonyRank> prizeCeremonySequence;

    private void Start()
    {
        simpleText.text = "";
        simpleImage.enabled = false;
        compositeText.text = "";
        compositeImage.enabled = false;

        prizeCeremonySequence =
            new List<CeremonyRank>(players.GetPlayers().Count);
        CreateCeremonyPrizeRanks();
    }

    public void StartCeremony()
    {
        StartCoroutine(StartDisplayConversation());
    }

    private void CreateCeremonyPrizeRanks()
    {
        foreach (int playerID in players.GetPlayers().Keys)
        {
            PlayerInfo info = players.GetPlayers()[playerID];
            PlayerStats playerStats = info.playerStats;
            prizeCeremonySequence.Add(new CeremonyRank(0, playerStats));
        }

        PrizeRankComparer comparer = new PrizeRankComparer();
        prizeCeremonySequence.Sort (comparer);

        int _rank = 1;
        prizeCeremonySequence[0].rank = 1;
        _rank = 2;
        for (int i = 1; i < prizeCeremonySequence.Count; i++)
        {
            if (
                prizeCeremonySequence[i].playerStats.masks ==
                prizeCeremonySequence[i - 1].playerStats.masks
            )
            {
                prizeCeremonySequence[i].rank =
                    prizeCeremonySequence[i - 1].rank;
            }
            else
            {
                prizeCeremonySequence[i].rank = _rank;
            }
            _rank += 1;
        }
    }

    IEnumerator StartDisplayConversation()
    {
        // ceremony opening
        foreach (ConversationObject conversation in openingConversations)
        {
            switch (conversation.conversationType)
            {
                case ConversationType.simpleText:
                    simpleText.text = conversation.text;
                    simpleText.color = conversation.textColor;
                    yield return new WaitForSeconds(conversation
                                .delayInSeconds);
                    simpleText.text = "";
                    break;
                case ConversationType.simpleImage:
                    simpleImage.enabled = true;
                    simpleImage.sprite = conversation.image;
                    yield return new WaitForSeconds(conversation
                                .delayInSeconds);
                    simpleImage.enabled = false;
                    break;
                case ConversationType.textAndImage:
                    compositeImage.enabled = true;
                    compositeText.text = conversation.text;
                    compositeText.color = conversation.textColor;
                    compositeImage.sprite = conversation.image;
                    yield return new WaitForSeconds(conversation
                                .delayInSeconds);
                    compositeText.text = "";
                    compositeImage.enabled = false;
                    break;
            }
        }

        // prize ceremony
        bool samePrize = false;
        for (int idx = 0; idx < prizeCeremonySequence.Count; idx++)
        {
            CeremonyRank ceremonyRank = prizeCeremonySequence[idx];
            PlayerStats playerStats = ceremonyRank.playerStats;
            if (idx > 0)
            {
                int prevRank = prizeCeremonySequence[idx - 1].rank;
                if (ceremonyRank.rank == prevRank)
                {
                    samePrize = true;
                }
                else
                {
                    samePrize = false;
                }
            }
            string _text = "";
            switch (ceremonyRank.rank)
            {
                case 1:
                    _text =
                        samePrize
                            ? "And We Have Another Champion..."
                            : "We Have Our Champion...";
                    break;
                case 2:
                    _text =
                        samePrize
                            ? "Another First Runner Up..."
                            : "First Runner Up...";
                    break;
                case 3:
                    _text =
                        samePrize
                            ? "Another Second Runner Up..."
                            : "Second Runner Up...";
                    break;
                case 4:
                    _text =
                        samePrize
                            ? "Another Third Runner Up..."
                            : "Third Runner Up...";
                    break;
            }
            SendSimpleText (_text, prizeCeremonyTextColor);
            yield return new WaitForSeconds(5);
            simpleText.text = "";
            compositeImage.enabled = true;
            SendTextImage(string.Format("{0}P", playerStats.playerID),
            playerStats.playerAccent,
            playerStats.playerAvatar);
            yield return new WaitForSeconds(5);
            compositeImage.enabled = false;
            compositeText.text = "";
            SendSimpleText("Obtained...", playerStats.playerAccent);
            yield return new WaitForSeconds(3);
            simpleText.text = "";
            compositeImage.enabled = true;
            SendTextImage(playerStats.masks.ToString(),
            playerStats.playerAccent,
            maskSprite);
            yield return new WaitForSeconds(5);
            compositeImage.enabled = false;
            compositeText.text = "";
        }

        // ceremony closing
        foreach (ConversationObject conversation in closingConversations)
        {
            switch (conversation.conversationType)
            {
                case ConversationType.simpleText:
                    simpleText.text = conversation.text;
                    simpleText.color = conversation.textColor;
                    yield return new WaitForSeconds(conversation
                                .delayInSeconds);
                    simpleText.text = "";
                    break;
                case ConversationType.simpleImage:
                    simpleImage.enabled = true;
                    simpleImage.sprite = conversation.image;
                    yield return new WaitForSeconds(conversation
                                .delayInSeconds);
                    simpleImage.enabled = false;
                    break;
                case ConversationType.textAndImage:
                    compositeImage.enabled = true;
                    compositeText.text = conversation.text;
                    compositeText.color = conversation.textColor;
                    compositeImage.sprite = conversation.image;
                    yield return new WaitForSeconds(conversation
                                .delayInSeconds);
                    compositeText.text = "";
                    compositeImage.enabled = false;
                    break;
            }
        }

        onPlayNextMiniGame.Fire();
    }

    private void SendSimpleText(string text, Color textColor)
    {
        simpleText.text = text;
        simpleText.color = textColor;
    }

    private void SendSimpleImage(Sprite image)
    {
        simpleImage.sprite = image;
    }

    private void SendTextImage(string text, Color textColor, Sprite image)
    {
        compositeText.text = text;
        compositeText.color = textColor;
        compositeImage.sprite = image;
    }

    private class CeremonyRank
    {
        public int rank;

        public PlayerStats playerStats;

        public CeremonyRank(int _rank, PlayerStats _playerStats)
        {
            rank = _rank;
            playerStats = _playerStats;
        }
    }

    private class PrizeRankComparer : IComparer<CeremonyRank>
    {
        public int Compare(CeremonyRank a, CeremonyRank b)
        {
            return -1 * a.playerStats.masks.CompareTo(b.playerStats.masks); // descending order
        }
    }
}
