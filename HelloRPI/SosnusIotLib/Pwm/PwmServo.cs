﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SosnusIotLib.Pwm
{
    public class PwmServo : PwmBasic
    {
        //frequency and width are in PwmBasic


        private double fillMin; // typically 0.3ms //mean 1,5% of fill
        private double fillMax; // typically 2.3ms //mean 11,5% of fill
        private double fillDelta;     // set in constructor

        double angleMax; //typically 120 or 150


        public PwmServo() { }

        public async void SetupServo(int _pinNumber)
        {
            fillMin = 0.3; // typically 0.3
            fillMax = 2.3; // typically 2.3
            fillDelta = fillMax - fillMin;

            Frequency = 50; //most of servos have 50Hz, 20ms

            angleMax = 120; //typically 120 or 150

            await SetupBasic(_pinNumber, Frequency);

        }



        public enum PwmInputType
        {
            ServoAngle,
            ServoFill,
            PwmFill //Ahtung!
        }

            private double fillTemp;

        public void Set(double variable, PwmInputType type)
        {
            fillTemp = 0; //na wszelki wypadek
            switch (type)
            {
                case PwmInputType.ServoAngle:
                    { //now Var can have between <0-120(150?)> but this isn't tested
                        fillTemp = (variable * fillDelta)/angleMax ;
                        fillTemp += fillMin;
                        fillTemp = (fillTemp * angleMax) / FrequencyToMiliseconds(Frequency); //change to Percent of fill (between 1.5% to 11.5%
                    }
                    break;
                case PwmInputType.ServoFill:
                    {
                        fillTemp = (variable * fillDelta) / 100; //fillTemp - how many ms i need add to fillMin?
                        fillTemp += fillMin; //fillTemp - was in ms, now add fillMin [ms]
                        fillTemp = (fillTemp*100)/ FrequencyToMiliseconds(Frequency); //change to Percent of fill (between 1.5% to 11.5%
                    }
                    break;
                case PwmInputType.PwmFill:
                    {
                        fillTemp = variable;
                    }
                    break;
            }
            //btween 0.3 to 2.3ms if not used PwmFill
            Set(fillTemp);
            
        }


    }
}