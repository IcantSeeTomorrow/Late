using Common.Data;
using Managers;
using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class NPCController : MonoBehaviour
{
    public int npcID;
    private bool inInteractive = false;

    Animator anim;
    NpcDefine npc;
    Color originColor;
    new SkinnedMeshRenderer renderer;

    private void Start()
    {
        anim = this.gameObject.GetComponent<Animator>();
        npc = NPCManager.Instance.GetNpcDefine(npcID);
        renderer = this.gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
        originColor = renderer.sharedMaterial.color;
        StartCoroutine(Actions());
    }

    private void Update()
    {

    }

    IEnumerator Actions()
    {
        while (true)
        {
            if (inInteractive)
                yield return new WaitForSeconds(2f);//交互中，不会Relax
            else
                yield return new WaitForSeconds(Random.Range(5f, 10f));//非交互，一小段时间做一次Relax
            Relax();
        }
    }

    void Relax()
    {
        anim.SetTrigger("Relax");

    }

    void Interactive()
    {
        if (!inInteractive)//避免重复点击
        {
            inInteractive = true;
            StartCoroutine(DoInteractive());
        }
    }

    IEnumerator DoInteractive()
    {
        yield return FaceToPlayer();
        if (NPCManager.Instance.Interactive(npc))//交互成功->Talk
        {
            anim.SetTrigger("Talk");
        }
        yield return new WaitForSeconds(3f);//3S交互冷却
        inInteractive = false;
    }

    IEnumerator FaceToPlayer()
    {
        Vector3 factTo = (User.Instance.CurrentCharacterObject.transform.position - this.gameObject.transform.position).normalized;//得到方向
        while (Mathf.Abs(Vector3.Angle(this.gameObject.transform.forward, factTo)) > 5)
        {
            this.gameObject.transform.forward = Vector3.Lerp(transform.forward, factTo, Time.deltaTime * 5f);
            yield return null;
        }
    }

    private void OnMouseDown()
    {
        Interactive();
    }

    private void OnMouseOver()
    {
        Highlight(true);
    }

    private void OnMouseEnter()
    {
        Highlight(true);
    }

    private void OnMouseExit()
    {
        Highlight(false);
    }

    void Highlight(bool highlight)
    {
        if (highlight)
        {
            if (renderer.sharedMaterial.color != Color.white)
                renderer.sharedMaterial.color = Color.white;
        }
        else
        {
            if (renderer.sharedMaterial.color != originColor)
                renderer.sharedMaterial.color = originColor;
        }

    }

}
