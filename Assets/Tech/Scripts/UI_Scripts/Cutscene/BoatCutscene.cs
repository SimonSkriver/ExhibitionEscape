using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class BoatCutscene : MonoBehaviour
{
    [SerializeField] PlayableDirector cutscene;
    Material flagMat, shortsMat;

    public bool inCutscene { get; private set; }

    void Awake()
    {
        flagMat = Resources.Load<Material>("Art/Materials/Colors/Flag");
        shortsMat = Resources.Load<Material>("Art/Materials/PlayerShorts");

        flagMat.color = new Color(0.9063354f, 0.2156203f, 0.1827435f, 1f);

        // Bind the cutscene to the Boat event
        // When Boat event is triggered, TryStartCutscene() will run
        EventManager.Instance.Boat += TryStartCutscene;
    }

    public void TryStartCutscene()
    {
        //if (!hat.hasHat) return;
        flagMat.color = shortsMat.color;
        cutscene.Play();
        inCutscene = true;
        UI_Manager.Instance.HidePlayerHUD();
        //StartCoroutine(TriggerNOOOOO());
    }
/*
    IEnumerator TriggerNOOOOO() {
        Animator anim = GameObject.FindWithTag("Player").GetComponent<Animator>();
        
        yield return new WaitForSeconds(19.25f);

        anim.SetTrigger("NOOOOO");

        yield return new WaitForSeconds(7f);
        UI_Manager.Instance.ShowCredits();
        Time.timeScale = 0;
    }*/
}