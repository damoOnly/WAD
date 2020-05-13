using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    /// <summary>
    /// 气体类型
    /// </summary>
    public enum EM_GasType
    {
        可燃气体_EX= 1,
        二氧化碳_CO2 = 2,
        一氧化碳_C0 = 3,
        氧气_O2 = 4,
        硫化氢_H2S = 5,
        氨气_NH3 = 6,
        氢气_H2 = 7,
        甲醛_CH2O = 8,
        臭氧_O3 = 9,
        氯气_CL2 = 10,
        氮气_N2 = 11,
        二氧化硫_SO2 = 12,
        甲烷_CH4 = 13,
        TVOC = 14,
        VOC = 15,
        PID = 16,
        一氧化氮_N0 = 17,
        二氧化氮_N02 = 18,
        氮氧化物_N0X = 19,
        环氧乙烷_ETO = 20,
        磷化氢_PH3 = 21,
        二氧化氯_CLO2 = 22,
        氯化氢_HCL = 23,
        溴化氢_HBr = 24,
        氰化氢_HCN = 25,
        光气_COCL2 = 26,
        溴甲烷_CH3Br = 27,
        硫酰氟_SO2F2 = 28,
        苯_C6H6 = 29,
        甲苯_C7H8 = 30,
        二甲苯_C8H10 = 31,
        丙烷_C3H8 = 32,
        硅烷_SiH4 = 33,
        氟气_F2 = 34,
        氟化氢_HF = 35,
        氦气_HE = 36,
        氩气_AR = 37,
        乙硼烷_B2H6 = 38,
        锗烷_GeH4 = 39,
        四氢噻吩_THT = 40,
        肼联氨_N2H4 = 41,
        乙烯_C2H4 = 42,
        乙烷_CH3CH3 = 43,
        砷化氢_AsH3 = 44,
        溴气_Br2 = 45,
        乙炔_C2H2 = 46,
        一氧化二氮_N2O = 47,
        六氟化硫_SF6 = 48,
        三氯化硼_BCL3 = 49,
        二硫化碳_CS2 = 50,
        乙醛_C2H4O = 51,
        丙烯腈_C3H3N = 52,
        丁二烯_C4H6 = 53,
        甲醇_CH4O = 54,
        甲胺_CH3NH2 = 55,
        甲硫醇_CH3SH = 56,
        甲硫醚_C2H6 = 57,
        二甲二硫醚_C2H6S2 = 58,
        氯乙烯_C2H3CL = 59,
        异丙醇_C3H8O = 60,
        乙醇_C2H6O = 61,
        丙酮_CH3COCH3 = 62,
        丁酮_C2H6O = 63,
        氯甲烷_CH3CL = 64,
        氯化苄_C6H5CH2CL = 65,
        二甲胺_C2H7N = 66,
        苯乙烯_C8H8 = 67,
        丁烷_C4H10 = 68,
        碳氢_HC = 69
    }

    /// <summary>
    /// 单位类型
    /// </summary>
    //public enum EM_UnitType:byte
    //{
    //    ppm = 0x00,
    //    vol = 0x01,
    //    LEL = 0x02,
    //    mgm3 = 0x03
    //}

    /// <summary>
    /// 错误类型
    /// </summary>
    public enum EM_Error : byte
    {
        GNM = 0x01,
        JCQAddress = 0x02,
        JCQNum = 0x03,
        JCQWrite = 0x04
    }


    /// <summary>
    /// 读取命令高字节
    /// </summary>
    public enum EM_AdrType : byte
    {
        设备工作状态_设备自检信息 = 0x00,
        设备工作状态_传感器在线状态 = 0x01,
        设备工作状态_传感器标定状态 = 0x02,
        设备工作状态_传感器工作状态 = 0x03,
        设备工作状态_传感器故障状态 = 0x04,
        气体浓度 = 0x05,      
        报警状态 = 0x07,
        气体名称 = 0x08,
        浓度值放大倍数 = 0x11,
        气体单位 = 0x12,
        气体低报点 = 0x13,
        气体高报点 = 0x15,
    }

    /// <summary>
    /// 读取命令高字节
    /// </summary>
    public enum EM_HighType : byte
    {
        通用 = 0x00,
        电化学氧气传感器 = 0x01,
        催化可燃气体传感器 = 0x02,
        电化学有毒气体A传感器 = 0x03,
        红外传感器 = 0x04,
        PID传感器 = 0x05,
        电化学有毒气体B传感器 = 0x06,
    }

    /// <summary>
    /// 读取命令,通用低字节
    /// </summary>
    public enum EM_LowType_T : byte
    {
        控制器报警开关 = 0x15,
        通讯测试命令 = 0x16,
        年份 = 0x17,
        月日 = 0x18,
        时分 = 0x19,
        秒 = 0x1a,
        仪器地址 = 0x1b,
        设备状态 = 0x1c,
        报警开关 = 0x1d,
        //报警状态读取 = 0x1e,
        设备温度读取 = 0x1f,
        设备湿度读取 = 0x21,
        调试命令 = 0x23,
        数据保存方式 = 0x24,
        数据上传 = 0x25,
        报警记录读取 = 0x26,
        恢复出厂设置 = 0x27,
        气泵开关 = 0x2c,
        气泵转速 = 0x2d,
        仪器语言 = 0x2f,
        温度补偿值 = 0x30,
        湿度补偿值 = 0x32,
        蓝牙开关 = 0x34,
        仪器中无线气泵及蓝牙 = 0x35,
        设备类型 = 0x36,
        无线开关 = 0x37
    }

    /// <summary>
    /// 读取命令,用户低字节
    /// </summary>
    public enum EM_LowType_U : byte
    {
        气体类型及单位 = 0x30,
        量程 = 0x34,
        低浓度报警点 = 0x3f,
        A1报警点 = 0x41,
        A2报警点 = 0x43,
        实时AD值 = 0x4c,
        实时浓度 = 0x4e,
        报警状态 = 0x50,
        温度 = 0x51,
        湿度 = 0x53,
        传感器开关状态 = 0x55,
        小数点 = 0x56,
        零点校准 = 0x57,
        零点AD值 = 0x58,
        零点浓度值 = 0x5a,
        一级AD值 = 0x5c,
        一级浓度值 = 0x5e,
        二级AD值 = 0x60,
        二级浓度值 = 0x62,
        三级AD值 = 0x64,
        三级浓度值 = 0x66,
        报警响应 = 0x68,
        存储模式 = 0x69,
        存储周期 = 0x6a,
        放大倍数 = 0x6b,
        TWA报警点 = 0x6d,
        STELA报警点 = 0x6f,
        STEL报警点时长 = 0x71
    }

    /// <summary>
    /// 下位机保存类型
    /// </summary>
    public enum EM_SaveType : byte
    {
        None = 0x00,
        User = 0x01,
        Admin = 0x02
    }

    /// <summary>
    /// 1.	电化学氧气传感器类型
    /// </summary>
    public enum EM_Gas_One : byte
    {
        氧气 = 0x01,
        氮气 = 0x02
    }
    /// <summary>
    /// 2.	催化可燃气体传感器
    /// </summary>
    public enum EM_Gas_Two : byte
    {
        可燃气体 = 0x01,
        甲烷 = 0x02,
        氨气 = 0x03,
        氢气 = 0x04,
        乙炔 = 0x06,
        乙烯 = 0x06,
        乙醇 = 0x07,
        甲醇 = 0x08,
        苯 = 0x09,
        甲苯 = 0x0a,
        二甲苯 = 0x0b,
        苯乙烯 = 0x0c,
        氯乙烯 = 0x0d,
        三氯乙烯 = 0x0e,
        四氯乙烯 = 0x0f,
        硫酸氟 = 0x10
    }
    /// <summary>
    /// 3.	电化学有毒气体传感器A
    /// </summary>
    public enum EM_Gas_Three : byte
    {
        一氧化碳 = 0x01,
        二氧化碳 = 0x02,
        甲醛 = 0x03,
        臭氧 = 0x04,
        硫化氢 = 0x05,
        二氧化硫 = 0x06,
        一氧化氮 = 0x07,
        二氧化氮 = 0x08,
        氯气 = 0x09,
        氨气 = 0x0a,
        氢气 = 0x0b,
        氰化氢 = 0x0c,
        氯化氢 = 0x0d,
        磷化氢 = 0x0e,
        二氧化氯 = 0x0f,
        环氧乙烷 = 0x10,
        光气 = 0x11,
        硅烷 = 0x12,
        氟气 = 0x13,
        氟化氢 = 0x14,
        溴化氢 = 0x15,
        乙硼烷 = 0x16,
        砷化氢 = 0x17,
        锗烷 = 0x18,
        肼联氨 = 0x19,
        溴气 = 0x1a,
        乙烯 = 0x1b,
        乙醛 = 0x1c,
        二硫化碳 = 0x1d,
        丙烯腈 = 0x1e,
        甲胺 = 0x1f,
        碘气 = 0x20,
        氮氧化物 = 0x21,
        笑气 = 0x22,
        氦气 = 0x23,
        氩气 = 0x24
    }
    /// <summary>
    /// 4.	红外传感器
    /// </summary>
    public enum EM_Gas_Four : byte
    {
        可燃气体 = 0x01,
        甲烷 = 0x02,
        一氧化碳 = 0x03,
        二氧化碳 = 0x04,
        乙炔 = 0x05,
        六氟化硫 = 0x06,
        溴甲烷 = 0x07,
    }
    /// <summary>
    /// 5.	PID传感器
    /// </summary>
    public enum EM_Gas_Five : byte
    {
        PID = 0x01,
        VOC = 0x02,
        乙醇 = 0x03,
        甲醇 = 0x04,
        苯 = 0x05,
        甲苯 = 0x06,
        二甲苯 = 0x07,
        苯乙烯 = 0x08,
        氯乙烯 = 0x09,
        三氯乙烯 = 0x0a,
        四氯乙烯 = 0x0b,
        硫酸氟 = 0x0c
    }
    /// <summary>
    /// 6.	电化学有毒气体传感器B
    /// </summary>
    public enum EM_Gas_Six : byte
    {
        一氧化碳 = 0x01,
        二氧化碳 = 0x02,
        甲醛 = 0x03,
        臭氧 = 0x04,
        硫化氢 = 0x05,
        二氧化硫 = 0x06,
        一氧化氮 = 0x07,
        二氧化氮 = 0x08,
        氯气 = 0x09,
        氨气 = 0x0a,
        氢气 = 0x0b,
        氰化氢 = 0x0c,
        氯化氢 = 0x0d,
        磷化氢 = 0x0e,
        二氧化氯 = 0x0f,
        环氧乙烷 = 0x10,
        光气 = 0x11,
        硅烷 = 0x12,
        氟气 = 0x13,
        氟化氢 = 0x14,
        溴化氢 = 0x15,
        乙硼烷 = 0x16,
        砷化氢 = 0x17,
        锗烷 = 0x18,
        肼联氨 = 0x19,
        溴气 = 0x1a,
        乙烯 = 0x1b,
        乙醛 = 0x1c,
        二硫化碳 = 0x1d,
        丙烯腈 = 0x1e,
        甲胺 = 0x1f,
        碘气 = 0x20,
        氮氧化物 = 0x21,
        笑气 = 0x22,
        氦气 = 0x23,
        氩气 = 0x24
    }

    public enum EM_AlertType : byte
    {
        超量程 = 1,
        低报警 = 2,
        A2报警 = 3,
        A1报警 = 4
    }

    public enum EM_UserType : byte
    {
        User=0,
        Admin = 1,
        Super = 2
    }
}
