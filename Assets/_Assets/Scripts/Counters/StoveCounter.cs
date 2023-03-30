using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CuttingCounter;

public class StoveCounter : BaseCounter
{
    private enum State
    {
        Idle,
        Frying,
        Fried,
        Burned,
    }

    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;
    

    private State state;
    private float fryingTimer;
    private float burningTimer;
    private FryingRecipeSO fryingRecipeSO;
    private BurningRecipeSO burningRecipeSO;

    private void Start()
    {
        state = State.Idle;
    }
    private void Update()
    {
        switch (state)
        {
            case State.Idle:
                break;
            case State.Frying:
                fryingTimer += Time.deltaTime;
                if (fryingTimer > fryingRecipeSO.fryingTimeMax)
                {
                    //bum

                    fryingTimer = 0;
                    Debug.Log("fried");
                    GetKitchenObject().DestroySelf();
                    KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);

                    Debug.Log("Object Fried");


                    state = State.Fried;
                    burningTimer = 0;
                    burningRecipeSO = GetBurningRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                }
                break;
            case State.Fried:
                burningTimer += Time.deltaTime;
                if (burningTimer > burningRecipeSO.burningTimerMax)
                {
                    //bum

                    fryingTimer = 0;
                    Debug.Log("fried");
                    GetKitchenObject().DestroySelf();
                    KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);

                    Debug.Log("Object Burned");
                    state = State.Burned;
                }
                break;
            case State.Burned:
                break;
            default:
                break;
        }

        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO()))
        {
            
        }
        Debug.Log(state);

    }
    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            //There is no KitchenObject here
            if (player.HasKitchenObject())
            {
                //Player is carrying something
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    //Player carrying something that can be fried
                    player.GetKitchenObject().SetKitchenObjectParent(this);

                    fryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                    state = State.Frying;
                    fryingTimer = 0;
                }

            }
            else
            {
                //Player not carrying anything
            }
        }
        else
        {
            //There is a KitchenObject here
            if (player.HasKitchenObject())
            {
                //Player is carrying something
            }
            else
            {
                //Player is not carrying anything

                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }

    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        return fryingRecipeSO != null;
    }
    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        if (fryingRecipeSO != null)
        {
            return fryingRecipeSO.output;
        }
        else
        {
            return null;
        }
    }
    private BurningRecipeSO GetBurningRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (BurningRecipeSO burningRecipeObjectSO in burningRecipeSOArray)
        {
            if (inputKitchenObjectSO == burningRecipeObjectSO.input)
            {
                return burningRecipeObjectSO;
            }
        }
        return null;
    }

    private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (FryingRecipeSO fryingRecipeObjectSO in fryingRecipeSOArray)
        {
            if (inputKitchenObjectSO == fryingRecipeObjectSO.input)
            {
                return fryingRecipeObjectSO;
            }
        }
        return null;
    }
}
