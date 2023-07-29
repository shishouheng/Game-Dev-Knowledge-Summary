using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    private SkinnedMeshRenderer skinnedMeshRenderer;
    public Texture[] skinTextures;
    public Animation ani;
    public Transform[] weapons;
    public Transform wpHand;
    private void Awake()
    {
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        ani = GetComponent<Animation>();
    }
    public void ChangeSkin(int index)
    {
        skinnedMeshRenderer.material.mainTexture = skinTextures[index];
    }
    public void ChangeAni(string aniName)
    {
        ani.Play(aniName);
    }
    public void ChangeWeapon(int index)
    {
        if(wpHand.childCount>=2)
        {
            DestroyImmediate(wpHand.GetChild(1).gameObject);
        }
        GameObject mWeapon=Instantiate<GameObject>(weapons[index].gameObject, wpHand.position, wpHand.rotation);
        mWeapon.transform.SetParent(wpHand);
    }
}
