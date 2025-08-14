using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StaminaScript : MonoBehaviour
{
    #region Variables
    [SerializeField]private float maxStamina = 100f;
    
    [SerializeField]private float staminaRegenRate = 5f;

    public float currentStamina;

    public float timeToRegen;
    private bool ableToRegen;
    public bool tired;
    public float lastUse;

    public float timeDiference;

    [SerializeField] Image staminaBar;
    [SerializeField] Image subStaminaBar;
    private Color staminaBarColor;
    #endregion

    public void Start()
    {
        staminaBarColor = staminaBar.color;
        currentStamina = maxStamina;
    }

    public void Update()
    {
        Regen();

    }

    public void Regen() {
        if (ableToRegen)
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
            updateUIRegen();

            if (currentStamina == maxStamina)
            {
                tired = false;
            }
        }
    }

    public void AbleRegen() { 
        StartCoroutine(RegenCountdown());
    }

    private IEnumerator RegenCountdown()
    {
        yield return new WaitForSeconds(timeToRegen);
        ableToRegen = true;
    }

    public void UNAbleRegen()
    {
        StopCoroutine(RegenCountdown());
        ableToRegen = false;
    }

    public void InstaAbleRegen()
    {
        ableToRegen = true;
    }

    public bool UseStamina(float amount)
    {
        if (currentStamina >= amount)
        {
            StopCoroutine(RegenCountdown());
            currentStamina -= amount;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
            ableToRegen = false;
            StartCoroutine(updateUI());
            return true;
        }
        else
        {
            StartCoroutine(notEnoughFeedback());
            return false;
        }
    }


    IEnumerator updateUI()
    {
        staminaBar.DOFillAmount(currentStamina / maxStamina, 0.1f).SetEase(Ease.Linear);
        yield return new WaitForSeconds(timeDiference);
        subStaminaBar.DOFillAmount(currentStamina / maxStamina, 0.1f).SetEase(Ease.Linear);

    }

    public void updateUIRegen()
    {
        staminaBar.DOFillAmount(currentStamina / maxStamina, 0.1f).SetEase(Ease.Linear);
        subStaminaBar.DOFillAmount(currentStamina / maxStamina, 0.1f).SetEase(Ease.Linear);
    }

    IEnumerator notEnoughFeedback()
    {
        staminaBar.DOColor(Color.red, 0.1f).SetEase(Ease.Linear);
        yield return new WaitForSeconds(0.1f);
        staminaBar.DOColor(staminaBarColor, 0.1f).SetEase(Ease.Linear);
    }

}
