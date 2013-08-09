using System;
using System.Collections.Generic;
using DNT.Engine.Core.Validation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DNT.Engine.Core.Context
{
    public class Environment
    {
        private const Int32 MaximumNumberOfLights = 3;

        static Environment()
        {
            Lock = new Object();
        }

        public static Environment Current()
        {
            if (_instance.IsNull())
                lock (Lock)
                    if (_instance.IsNull())
                        _instance = new Environment();

            return _instance;
        }

        private static Environment _instance;

        private Environment()
        {
            _lights = new List<DirectionalLight>();
        }

        public Vector3 AmbientLightColor
        {
            get { return _ambientLightColor; }
            set { _ambientLightColor = value; }
        }
        private Vector3 _ambientLightColor;

        public Environment AddLight(DirectionalLight light)
        {
            if (_lights.Count > MaximumNumberOfLights)
                throw new NotSupportedException(String.Format("A maximum of {0} environment lights are supported.", MaximumNumberOfLights));

            _lights.Add(light);
            return this;
        }

        public void TurnLightsOn()
        {
            _lightsOff = false;
        }

        public void TurnLightsOff()
        {
            _lightsOff = true;
        }

        private readonly IList<DirectionalLight> _lights;
        private static readonly Object Lock;
        private Boolean _lightsOff;

        public Environment EnableDefaultLights()
        {
            _ambientLightColor = new Vector3(0.05333332f, 0.09882354f, 0.1819608f);
            _lights.Clear();
            _lights.Add(new DirectionalLight
                            {
                                DiffuseColor = new Vector3(1.0f, 0.9607844f, 0.8078432f),
                                Direction = new Vector3(-0.5265408f, -0.5735765f, -0.6275069f),
                                SpecularColor = new Vector3(1.0f, 0.9607844f, 0.8078432f)
                            });
            _lights.Add(new DirectionalLight
                            {
                                DiffuseColor = new Vector3(0.9647059f, 0.7607844f, 0.4078432f),
                                Direction = new Vector3(0.7198464f, 0.3420201f, 0.6040227f),
                                SpecularColor = new Vector3(0.0f, 0.0f, 0.0f)
                            });
            _lights.Add(new DirectionalLight
            {
                DiffuseColor = new Vector3(0.3231373f, 0.3607844f, 0.3937255f),
                Direction = new Vector3(0.4545195f, -0.7660444f, 0.4545195f),
                SpecularColor = new Vector3(0.3231373f, 0.3607844f, 0.3937255f)
            });

            return this;
        }

        public void SetLights(IEffectLights effect)
        {
            effect.AmbientLightColor = _ambientLightColor;

            // HACK: Environent light doesn't allow lights off.
            if (!(effect is EnvironmentMapEffect))
                effect.LightingEnabled = !_lightsOff;

            SetLight(effect.DirectionalLight0, 0);
            SetLight(effect.DirectionalLight1, 1);
            SetLight(effect.DirectionalLight2, 2);
        }

        private void SetLight(Microsoft.Xna.Framework.Graphics.DirectionalLight light, Int32 index)
        {
            Verify.That(light).Named("light").IsNotNull();

            if (index >= _lights.Count)
            {
                light.Enabled = false;
                return;
            }

            light.Enabled = true;
            light.DiffuseColor = _lights[index].DiffuseColor;
            light.Direction = _lights[index].Direction;
            light.SpecularColor = _lights[index].SpecularColor;
        }
    }
}