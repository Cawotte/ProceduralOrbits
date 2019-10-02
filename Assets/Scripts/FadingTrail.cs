using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class FadingTrail : Orbital
{

    [SerializeField] private float fadingTime = 2f;
    private float currentLifetime = 0f;

    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        rotatingSpeed = 0f;
        currentLifetime = 0f;
        SetAlpha(0f);
    }

    // Update is called once per frame
    protected void FixedUpdate()
    {
        
        SetAlpha(Mathf.Lerp(1, 0, currentLifetime / fadingTime));
        if (currentLifetime >= fadingTime)
        {
            Destroy(gameObject);
            return;
        }

        currentLifetime += Time.deltaTime;

        transform.position = GetPositionOnCircle(currentAngle);

    }


    public void InitializeTrail(
        Orbital parentOrbital,
        float fadingTime)
    {
        this.fadingTime = fadingTime;
        this.orbitRadius = parentOrbital.OrbitRadius;
        this.centerOfGravity = parentOrbital.CenterOfGravity;
        this.currentAngle = parentOrbital.CurrentAngle;
        rotatingSpeed = 0f;
    }

    
    private void SetAlpha(float alpha)
    {
        Color color = sr.color;
        color.a = alpha;
        sr.color = color;
    }
}
