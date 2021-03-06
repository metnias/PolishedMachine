using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using RWCustom;
using PolishedMachine.Config;

namespace OptionalUI
{
    /// <summary>
    /// Configuable Settings. Every configuable value is tied to UIconfig and Key.
    /// UIconfig value will be saved automatically when it is added to the tab.
    /// </summary>
    public class UIconfig : UIelement
    {
        /// <summary>
        /// Rectangular UIconfig.
        /// </summary>
        /// <param name="pos">BottomLeft Position</param>
        /// <param name="size">Size</param>
        /// <param name="key">Key: this must be unique</param>
        /// <param name="defaultValue">Default Value</param>
        public UIconfig(Vector2 pos, Vector2 size, string key, string defaultValue = "") : base(pos, size)
        {
            if (string.IsNullOrEmpty(key) || key.Substring(0, 1) == "_") { this.cosmetic = true; this.key = "_"; }
            else { this.cosmetic = false; this.key = key; }
            this._value = defaultValue;
            this.defaultValue = this._value;
            this.greyedOut = false;
            this.held = false;
        }
        /// <summary>
        /// Circular UIconfig.
        /// </summary>
        /// <param name="pos">BottomLeft Position (NOT center!)</param>
        /// <param name="rad">Radian</param>
        /// <param name="key">Key: this must be unique</param>
        /// <param name="defaultValue">Default Value</param>
        public UIconfig(Vector2 pos, float rad, string key, string defaultValue = "") : base(pos, rad)
        {
            if (string.IsNullOrEmpty(key) || key.Substring(0, 1) == "_") { this.cosmetic = true; this.key = "_"; }
            else { this.cosmetic = false; this.key = key; }
            this._value = defaultValue;
            this.defaultValue = this._value;
            this.greyedOut = false;
            this.held = false;
        }

        internal string defaultValue;
        public override void Reset()
        {
            base.Reset();
            this.value = this.defaultValue;
            this.held = false;
        }

        /// <summary>
        /// Set key to "_" to make this UIconfig cosmetic and don't save its value
        /// </summary>
        public readonly bool cosmetic;

        /// <summary>
        /// Whether this is held or not.
        /// If this is true, this freezes other objects.
        /// </summary>
        public bool held
        {
            get { return _held; }
            set
            {
                if (_held != value)
                {
                    _held = value;
                    ConfigMenu.freezeMenu = value;
                }
            }
        }
        private bool _held;

        /// <summary>
        /// Unique Key for this UIconfig
        /// </summary>
        public readonly string key;

        /// <summary>
        /// Whether this UIconfig is greyedOut or not
        /// </summary>
        public bool greyedOut;

        /// <summary>
        /// If you want to change value w/o running OnChange().
        /// This is not recommended unless you know what you are doing.
        /// </summary>
        public void ForceValue(string newValue)
        {
            this._value = newValue;
        }

        private string _value;
        /// <summary>
        /// Value.
        /// </summary>
        public virtual string value
        {
            get
            {
                return _value;
            }
            set
            {
                if (_value != value)
                {
                    _value = value;
                    if (_init) { OnChange(); }
                }
            }
        }

        /// <summary>
        /// Value in integer form
        /// </summary>
        public int valueInt
        {
            get { return int.TryParse(value, out int i) ? i : 0; }
            set { this.value = value.ToString(); }
        }

        /// <summary>
        /// Value in float form
        /// </summary>
        public float valueFloat
        {
            get
            {
                return float.TryParse(value, out float d) ? d : 0f;
            }
            set
            {
                this.value = value.ToString();
            }
        }

        /// <summary>
        /// Value in bool form
        /// </summary>
        public bool valueBool
        {
            set { this.value = value ? "true" : "false"; }
            get
            {
                if (this.value == "true") { return true; }
                else { return false; }
            }
        }

        internal override void OnChange()
        {
            base.OnChange();
            OptionScript.configChanged = true;
            (menu as ConfigMenu).saveButton.menuLabel.text = "APPLY";
        }

        /// <summary>
        /// Separates Graphical update for code-visiblilty.
        /// </summary>
        /// <param name="dt">deltaTime</param>
        public override void GrafUpdate(float dt)
        {
            base.GrafUpdate(dt);

        }

        /// <summary>
        /// Update that happens every frame.
        /// </summary>
        /// <param name="dt">deltaTime</param>
        public override void Update(float dt)
        {
            if (!_init) { return; }
            base.Update(dt);
            if (showDesc && !this.greyedOut)
            {
                ConfigMenu.description = this.description;
            }
        }

    }




}
