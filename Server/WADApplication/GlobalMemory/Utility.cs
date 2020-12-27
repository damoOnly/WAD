using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalMemory
{
    public class Utility
    {
        public static string GetGasName(byte gasType)
        {
            DictionaryFieldValue gas = null;
            if (CommonMemory.IsOldVersion && CommonMemory.IsOlden)
            {
                gas = CommonMemory.Config.GasOldenName.Find(c=>c.Value == gasType);
            }
            else
            {
                gas = CommonMemory.Config.GasName.Find(c => c.Value == gasType);
            }
            
            return gas == null ? string.Empty : gas.Key;
        }

        public static string GetWeatherName(byte weatherType)
        {
            var weather = CommonMemory.Config.WeatherName.Find(c => c.Value == weatherType);
            return weather == null ? string.Empty : weather.Key;
        }

        public static string GetGasUnitName(byte unit)
        {
            var gas = CommonMemory.Config.GasUnit.Find(c => c.Value == unit);
            return gas == null ? string.Empty : gas.Key;
        }

        public static string GetWeatherUnitName(byte unit)
        {
            var weather = CommonMemory.Config.WeatherUnit.Find(c => c.Value == unit);
            return weather == null ? string.Empty : weather.Key;
        }

        public static Equipment ConvertToEq(StructEquipment eq)
        {
            Equipment result = new Equipment() { 
                ID = eq.ID,
                Name = eq.Name,
                Address = eq.Address,
                GasType = eq.GasType,
                SensorNum = eq.SensorNum,
                UnitType = eq.UnitType,
                Point = eq.Point,
                Magnification = eq.Magnification,
                A1 = eq.A1,
                A2 = eq.A2,
                Max = eq.Max,
                IsGas = eq.IsGas,
                IsDel = eq.IsDel,
                IsNew = eq.IsNew,
                AlertModel = eq.AlertModel,
                MN = eq.MN,
                AliasGasName = eq.AliasGasName,
                CreateTime = eq.CreateTime
            };
            if (result.IsGas)
            {
                result._gasName = GetGasName(result.GasType);
                result.UnitName = GetGasUnitName(result.UnitType);
            }
            else
            {
                result._gasName = GetWeatherName(result.GasType) + "(气象)";
                result.UnitName = GetWeatherUnitName(result.UnitType);
            }

            return result;
        }

        public static DateTime CutOffMillisecond(DateTime dt)
        {
            return new DateTime(dt.Ticks - (dt.Ticks % TimeSpan.TicksPerSecond), dt.Kind);
        }
    }
}
