using System;

namespace OBSAudioMixer
{
    public class AudioConversionHelper
    {
        private static float LogOffsetDB = 6.0f;
        private static float LogRangeDB = 96.0f;
        private static float LogOffsetVal = (float)-Math.Log10(LogOffsetDB);
        private static float LogRangeVal = (float)Math.Log10(-LogRangeDB + LogOffsetDB);

        public static float MulTodB(float num)
        {
            return (num == 0.0f) ? float.NegativeInfinity : (20.0f * (float)Math.Log10(num));
        }

        public static float dBtoMul(float num)
        {
            return IsFinite(num) ? (float)Math.Pow(10.0f, num / 20.0f) : 0.0f;
        }

        public static bool IsFinite(float num)
        {
            return !double.IsInfinity(num) && !double.IsNaN(num);
        }

        public static float CubicDefTodB(float def)
        {
            if (def == 1.0f)
                return 0.0f;
            else if (def <= 0.0f)
                return -180;

            return MulTodB(def * def * def);
        }

        public static float CubicdBtoDef(float db)
        {
            if (db == 0.0f)
                return 1.0f;
            else if (db <= -180)
                return 0.0f;

            return (float)Math.Pow(dBtoMul(db), (double)1 / 3);
        }

        public static float IECDefTodB(float def)
        {
            if (def == 1.0f)
                return 0.0f;
            else if (def <= 0.0f)
                return float.NegativeInfinity;

            float db;

            if (def >= 0.75f)
                db = (def - 1.0f) / 0.25f * 9.0f;
            else if (def >= 0.5f)
                db = (def - 0.75f) / 0.25f * 11.0f - 9.0f;
            else if (def >= 0.3f)
                db = (def - 0.5f) / 0.2f * 10.0f - 20.0f;
            else if (def >= 0.15f)
                db = (def - 0.3f) / 0.15f * 10.0f - 30.0f;
            else if (def >= 0.075f)
                db = (def - 0.15f) / 0.075f * 10.0f - 40.0f;
            else if (def >= 0.025f)
                db = (def - 0.075f) / 0.05f * 10.0f - 50.0f;
            else if (def >= 0.001f)
                db = (def - 0.025f) / 0.025f * 90.0f - 60.0f;
            else
                db = float.NegativeInfinity;

            return db;
        }

        public static float IECdBtoDef(float db)
        {
            if (db == 0.0f)
                return 1.0f;
            else if (db == float.NegativeInfinity)
                return 0.0f;

            float def;

            if (db >= -9.0f)
                def = (db + 9.0f) / 9.0f * 0.25f + 0.75f;
            else if (db >= -20.0f)
                def = (db + 20.0f) / 11.0f * 0.25f + 0.5f;
            else if (db >= -30.0f)
                def = (db + 30.0f) / 10.0f * 0.2f + 0.3f;
            else if (db >= -40.0f)
                def = (db + 40.0f) / 10.0f * 0.15f + 0.15f;
            else if (db >= -50.0f)
                def = (db + 50.0f) / 10.0f * 0.075f + 0.075f;
            else if (db >= -60.0f)
                def = (db + 60.0f) / 10.0f * 0.05f + 0.025f;
            else if (db >= -114.0f)
                def = (db + 150.0f) / 90.0f * 0.025f;
            else
                def = 0.0f;

            return def;
        }

        public static float LogDefTodB(float def)
        {
            if (def >= 1.0f)
                return 0.0f;
            else if (def <= 0.0f)
                return float.NegativeInfinity;

            return (float)(-(LogRangeDB + LogOffsetDB) * Math.Pow((LogRangeDB + LogOffsetDB) / LogOffsetDB, -def) + LogOffsetDB);
        }

        public static float LogdBToDef(float db)
        {
            if (db >= 0.0f)
                return 1.0f;
            else if (db <= -96.0f)
                return 0.0f;

            return (float)((-Math.Log10(-db + LogOffsetDB) - LogRangeVal) / (LogOffsetVal - LogRangeVal));
        }
    }
}