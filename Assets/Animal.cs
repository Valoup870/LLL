using UnityEngine;

public class Animal : MonoBehaviour
{
    private float hunger;
    public float hungerInterval = 8;
    private float hungerTime;

    private float thirst;
    public float thirstInterval = 6;
    private float thirstTime;

    private float ReproductiveUrge;
    public float ReproductiveUrgeInterval = 20;
    private float ReproductiveUrgeTime;

    public float moveSpeed = 2;

    public float desirability = 1;

    private bool male = true;

    private bool IsPregnant = false;
    private float gestation;
    public float gestationInterval;
    private float gestationTime;

    public float sensoryDistance = 8;

    private float caloriesProvided;

    private void Update()
    {
        hungerTime += Time.deltaTime;
        
        if (hungerTime >= hungerInterval)
        {
            hunger += 1;
            hungerTime = 0;
            Debug.Log("Hunger: "+ hunger, this);
        }
        if (hunger >= 100)
        {
            hunger = 100;
            //modifier ça pour rajouter des effets de GROSSE faim
            Debug.Log("This BOY IS STARVING", this);
        }
        if(hunger < 0)
        {
            hunger = 0;
        }
        
        thirstTime += Time.deltaTime;
        
        if (thirstTime >= thirstInterval)
        {
            thirst += 1;
            thirstTime = 0;
            Debug.Log("Thirst: " + thirst, this);
        }
        if (thirst >= 100)
        {
            thirst = 100;
            //modifier ça pour rajouter des effets de GROSSE soif
            Debug.Log("This BOY IS DYING OF NO WATER", this);
        }
        if (thirst < 0)
        {
            thirst = 0;
        }

        ReproductiveUrgeTime += Time.deltaTime;

        if (ReproductiveUrgeTime >= ReproductiveUrgeInterval)
        {
            ReproductiveUrge += 1;
            ReproductiveUrgeTime = 0;
            Debug.Log("ReproductiveUrge: " + ReproductiveUrge, this);
        }
        if (ReproductiveUrge >= 100)
        {
            ReproductiveUrge = 100;
            //modifier ça pour rajouter des effets de GROSSE faim
            Debug.Log("This BOY IS HORNY", this);
        }
        if (ReproductiveUrge < 0)
        {
            ReproductiveUrge = 0;
        }
    }
}
