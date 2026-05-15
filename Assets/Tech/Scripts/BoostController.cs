using UnityEngine;
using System.Collections;

public class BoostController : MonoBehaviour
{
    public static BoostController Instance;

    [Header("References")]
    [SerializeField] private Animator anim;

    public void UseBoost(ItemData itemData)
    {
        ItemType type = itemData.itemType;
        switch (type)
        {
            case ItemType.Coconut: 
                StartCoroutine(CoconutEffect(itemData));
                break;

            case ItemType.Banana:
                StartCoroutine(BananaEffect(itemData));
                break;

            case ItemType.Meat:
                StartCoroutine(MeatEffect(itemData));
                break;

            default:
            break;
        }
    }

    private IEnumerator CoconutEffect(ItemData itemData)
    {
        if (itemData is not FoodData foodData)
        {
            yield break;
        }

    anim.SetBool("CoconutEffect", true);
    PlayerStats.Instance.MovementSpeed += foodData.boostAmount;

    while (anim.GetCurrentAnimatorStateInfo(0).IsName("CoconutEffect") 
          && anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f) 
        {
            yield return null;
        }

    anim.SetBool("CoconutEffect", false);

    float boostedSpeed = PlayerStats.Instance.MovementSpeed;
    float normalSpeed = boostedSpeed - foodData.boostAmount;

    float declineDuration = 2f;
    float timer = 0f;

    while (timer < declineDuration)
        {
            timer += Time.deltaTime;

            float t = timer / declineDuration;

            PlayerStats.Instance.MovementSpeed = Mathf.Lerp(boostedSpeed, normalSpeed, t);

            yield return null;
        }

    PlayerStats.Instance.MovementSpeed = normalSpeed;

    }

    private IEnumerator BananaEffect(ItemData itemData)
    {
        if (itemData is not FoodData foodData)
        {
            yield break;
        }

    anim.SetBool("BananaEffect", true);
    PlayerStats.Instance.JumpPower += foodData.boostAmount;

    while (anim.GetCurrentAnimatorStateInfo(0).IsName("BananaEffect") 
          && anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f) 
        {
            yield return null;
        }

    anim.SetBool("BananaEffect", false);

    float boostedPower = PlayerStats.Instance.JumpPower;
    float normalPower = boostedPower - foodData.boostAmount;

    float declineDuration = 2f;
    float timer = 0f;

    while (timer < declineDuration)
        {
            timer += Time.deltaTime;

            float t = timer / declineDuration;

            PlayerStats.Instance.JumpPower = Mathf.Lerp(boostedPower, normalPower, t);

            yield return null;
        }

    PlayerStats.Instance.JumpPower = normalPower;
    
    }

    private IEnumerator MeatEffect(ItemData itemData)
    {
        if (itemData is not FoodData foodData)
        {
            yield break;
        }

    anim.SetBool("MeatEffect", true);
    //SoundEffectsManager.Instance.PlaySFX();
    PlayerStats.Instance.Strength += foodData.boostAmount;

    while (anim.GetCurrentAnimatorStateInfo(0).IsName("MeatEffect") 
          && anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f) 
        {
            yield return null;
        }

    anim.SetBool("MeatEffect", false);

    float boostedStrength = PlayerStats.Instance.Strength;
    float normalStrength = boostedStrength - foodData.boostAmount;

    float declineDuration = 2f;
    float timer = 0f;

    while (timer < declineDuration)
        {
            timer += Time.deltaTime;

            float t = timer / declineDuration;

            PlayerStats.Instance.Strength = Mathf.Lerp(boostedStrength, normalStrength, t);

            yield return null;
        }

    PlayerStats.Instance.Strength = normalStrength;
    }
}
