using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private Image barImage;
    [SerializeField] private GameObject hasProgressGameobject;

    private IHasProgress hasProgress;

    private void Start()
    {
        hasProgress = hasProgressGameobject.GetComponent<IHasProgress>();
        if (hasProgress == null)
        {
            Debug.LogError($"Game Object {hasProgressGameobject} does not have component that implements IHasProgress!");
        }

        hasProgress.OnProgressChanged += IHasProgress_OnProgessChanged;

        barImage.fillAmount = 0;

        Hide();
    }

    private void IHasProgress_OnProgessChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        barImage.fillAmount = e.progressNormalized;

        if (e.progressNormalized == 0 || e.progressNormalized == 1)
        {
            Hide();
        }
        else
        {
            Show();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
