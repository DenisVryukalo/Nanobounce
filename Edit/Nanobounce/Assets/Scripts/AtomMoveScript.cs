using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.UI;

public class AtomMoveScript : MonoBehaviour {

    public Transform AnotherAtom;
    private double charge;
    private double mass;
    private float dx;
    private float dy;
    private double distCoef;
    private double timeCoef;
    private float timeLaps;
    private float scaldx;
    private float scaldy;
    public Text scale;
    public Text force;

    public void myRestart(){
        System.Random rand = new System.Random();
        DateTime now = DateTime.Now;
        float sx, fluct;
        if (name == "Atom1")
            sx = (float)62.01;
        else
        {
            sx = (float)-65.19;
            rand.Next();
        }

        fluct = (rand.Next(21) - 10) * 100;
        
        transform.position = new Vector3(sx, 0 + (float)(fluct) / 100, 0);
        dx = scaldx * -1 * plusminus(transform.position.x);
		dy = 0;
	}

    public void setTimeLaps(float input)
    {
        if (input > 10)
            timeLaps = (input - 10) * (float)2.5;
        else
            timeLaps = input / 10;
        string stroutput = (timeLaps * 3) + "   ";
        scale.text = "Время Х " + stroutput.Substring(0, 4) + " * 10^-17";
    }

    public void setCharge(string input)
    {
        if(Convert.ToDouble(input) > 0)
		    charge = Convert.ToDouble(input) * 1.6 * Math.Pow(10, -19);
    }
    public void setMass(string input)
    {
        if(Convert.ToDouble(input) > 0)
		    mass = Convert.ToDouble(input) * 1.67 * Math.Pow(10, -27);
    }

    public void setScaldx(string input) {

        scaldx = (float)Convert.ToDouble(input) / 30;
		dx = scaldx * -1 * plusminus(transform.position.x);
    }
    public void setScaldy(float input)
    {
        scaldy = input;
    }

    private int plusminus(float x) {
        if (x == 0)
            return 0;
        return (int)(x / Math.Abs(x));
    }
    private int plusminus(double x)
    {
        return (int)(x / Math.Abs(x));
    }

    void Start() {
        System.Random rand = new System.Random();
        DateTime now = DateTime.Now;

            charge = 1;
            charge = charge * 1.6 * Math.Pow(10, -19);

        mass = 1;
        mass = mass * 1.67 * Math.Pow(10, -27);

        scaldx = (float)1 / 30;
        timeLaps = (float)0.6;

        distCoef = Math.Pow(10, -14);
        timeCoef = Math.Pow(10, -20);

        int fluct;
        if (name != "Atom1")
            rand.Next();
        fluct = (rand.Next(21) - 10) * 100;
        transform.position = new Vector3(transform.position.x, transform.position.y + (float)(fluct) / 100, 0);

        dx = scaldx * -1 * plusminus(transform.position.x);
        dy = 0;

        GetComponent<AtomMoveScript>().enabled = false;

        InvokeRepeating("step", 0, (float)(0.033));
    }

    void Update()
    {
        
    }

    private void step()
    {
        if((Math.Abs(transform.position.x) > 130 || Math.Abs(transform.position.y) > 100) || (!enabled))
            return ;

        double forceKul = (charge * charge) / (4 * Math.PI * 8.85 * Math.Pow(10, -12) * Math.Pow(Vector3.Distance(transform.position, AnotherAtom.position) * distCoef, 2));

        string stroutput = "" + forceKul * 10000;
        force.text = "Сила Кулона: " + stroutput.Substring(0, 5) + " *10^-4 Н";

        double boostKul = forceKul / mass;

        float differx = transform.position.x - AnotherAtom.position.x;
        float differy = transform.position.y - AnotherAtom.position.y;

        float ddxKul = (float)(timeLaps * boostKul * Math.Sin(Math.Atan(Math.Abs(differx / differy))) * Math.Pow(timeCoef, 2) * plusminus(differx) / distCoef);
        float ddyKul = (float)(timeLaps * boostKul * Math.Cos(Math.Atan(Math.Abs(differx / differy))) * Math.Pow(timeCoef, 2) * plusminus(differy) / distCoef);

        dx += ddxKul;
        dy += ddyKul;

        float timedx = dx * timeLaps;
        float timedy = dy * timeLaps;

        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x + timedx, transform.position.y + timedy, 0), (float)(Math.Sqrt(timedx * timedx + timedy * timedy)));
    }
}

