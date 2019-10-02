using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Orbital : MonoBehaviour
{

    [Header("Orbit settings")]
    [SerializeField] protected Transform centerOfGravity;
    [SerializeField] protected float rotatingSpeed;
    [SerializeField] protected float orbitRadius;
    [SerializeField] private bool isRotatingClockwise = true;
    [SerializeField] protected float relativeSize = 1f;

    protected float currentAngle = 0f;

    public float CurrentAngle => currentAngle;

    public Transform CenterOfGravity => centerOfGravity;

    public float OrbitRadius => orbitRadius;


    public void InitializeOrbital(
        Transform centerOfGravity,
        float rotatingSpeed,
        float orbitRadius,
        bool isRotatingClockwise,
        float relativeSize)
    {
        this.centerOfGravity = centerOfGravity;
        this.rotatingSpeed = rotatingSpeed;
        this.orbitRadius = orbitRadius;
        this.isRotatingClockwise = isRotatingClockwise;
        this.relativeSize = relativeSize;
    }



    private void Start()
    {
        SetRelativeSize(relativeSize);
        currentAngle = UnityEngine.Random.Range(0f, 360f);
    }

    // Update is called once per frame
    protected void FixedUpdate()
    {

        float clockwiseMultiplier = (isRotatingClockwise) ? 1f : -1f;
        
        //Move object as orbit
        currentAngle += rotatingSpeed * Time.deltaTime * clockwiseMultiplier;
        
        if (CurrentAngle >= 360f)
        {
            currentAngle = CurrentAngle - 360f * clockwiseMultiplier;
        }

        
        transform.position = GetPositionOnCircle(currentAngle);
        
    }

    protected Vector2 GetPositionOnCircle(float angle)
    {
        Vector2 centerPos = centerOfGravity.position;
        float x = centerPos.x + Mathf.Cos(angle) * orbitRadius;
        float y = centerPos.y + Mathf.Sin(angle) * orbitRadius;
        
        return new Vector2(x, y);
    }
    
    public void SetColor(Color color)
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        color.a = sr.color.a;
        sr.color = color;
    }
    public void SetRelativeSize(float relativeSize)
    {
        
        transform.localScale = new Vector3(
            relativeSize, 
            relativeSize, 
            0);
    }



    
}
