using System.Text.Json.Serialization;

namespace TeslaMateSolar.Data.Solar;

public class GrottState
{
    [JsonPropertyName("device")]
    public string Device { get; set; }

    [JsonPropertyName("time")]
    public DateTime Time { get; set; }

    [JsonPropertyName("buffered")]
    public string Buffered { get; set; }

    [JsonPropertyName("values")]
    public GrottValues Values { get; set; }

    public class GrottValues
    {
        [JsonPropertyName("pvstatus")]
        public int Pvstatus { get; set; }

        [JsonPropertyName("pvpowerin")]
        public int Pvpowerin { get; set; }

        [JsonPropertyName("pv1voltage")]
        public int Pv1voltage { get; set; }

        [JsonPropertyName("pv1current")]
        public int Pv1current { get; set; }

        [JsonPropertyName("pv1watt")]
        public int Pv1watt { get; set; }

        [JsonPropertyName("pv2voltage")]
        public int Pv2voltage { get; set; }

        [JsonPropertyName("pv2current")]
        public int Pv2current { get; set; }

        [JsonPropertyName("pv2watt")]
        public int Pv2watt { get; set; }

        [JsonPropertyName("pvpowerout")]
        public int Pvpowerout { get; set; }

        [JsonPropertyName("pvfrequentie")]
        public int Pvfrequentie { get; set; }

        [JsonPropertyName("pvgridvoltage")]
        public int Pvgridvoltage { get; set; }

        [JsonPropertyName("pvgridcurrent")]
        public int Pvgridcurrent { get; set; }

        [JsonPropertyName("pvgridpower")]
        public int Pvgridpower { get; set; }

        [JsonPropertyName("pvgridvoltage2")]
        public int Pvgridvoltage2 { get; set; }

        [JsonPropertyName("pvgridcurrent2")]
        public int Pvgridcurrent2 { get; set; }

        [JsonPropertyName("pvgridpower2")]
        public int Pvgridpower2 { get; set; }

        [JsonPropertyName("pvgridvoltage3")]
        public int Pvgridvoltage3 { get; set; }

        [JsonPropertyName("pvgridcurrent3")]
        public int Pvgridcurrent3 { get; set; }

        [JsonPropertyName("pvgridpower3")]
        public int Pvgridpower3 { get; set; }

        [JsonPropertyName("totworktime")]
        public int Totworktime { get; set; }

        [JsonPropertyName("eactoday")]
        public int Eactoday { get; set; }

        [JsonPropertyName("pvenergytoday")]
        public int Pvenergytoday { get; set; }

        [JsonPropertyName("eactotal")]
        public int Eactotal { get; set; }

        [JsonPropertyName("epvtotal")]
        public int Epvtotal { get; set; }

        [JsonPropertyName("epv1today")]
        public int Epv1today { get; set; }

        [JsonPropertyName("epv1total")]
        public int Epv1total { get; set; }

        [JsonPropertyName("epv2today")]
        public int Epv2today { get; set; }

        [JsonPropertyName("epv2total")]
        public int Epv2total { get; set; }

        [JsonPropertyName("pvtemperature")]
        public int Pvtemperature { get; set; }

        [JsonPropertyName("pvipmtemperature")]
        public int Pvipmtemperature { get; set; }

        [JsonPropertyName("pvboosttemp")]
        public int Pvboosttemp { get; set; }

        [JsonPropertyName("bat_dsp")]
        public int BatDsp { get; set; }

        [JsonPropertyName("eacharge_today")]
        public int EachargeToday { get; set; }

        [JsonPropertyName("eacharge_total")]
        public int EachargeTotal { get; set; }

        [JsonPropertyName("batterytype")]
        public int Batterytype { get; set; }

        [JsonPropertyName("uwsysworkmode")]
        public int Uwsysworkmode { get; set; }

        [JsonPropertyName("systemfaultword0")]
        public int Systemfaultword0 { get; set; }

        [JsonPropertyName("systemfaultword1")]
        public int Systemfaultword1 { get; set; }

        [JsonPropertyName("systemfaultword2")]
        public int Systemfaultword2 { get; set; }

        [JsonPropertyName("systemfaultword3")]
        public int Systemfaultword3 { get; set; }

        [JsonPropertyName("systemfaultword4")]
        public int Systemfaultword4 { get; set; }

        [JsonPropertyName("systemfaultword5")]
        public int Systemfaultword5 { get; set; }

        [JsonPropertyName("systemfaultword6")]
        public int Systemfaultword6 { get; set; }

        [JsonPropertyName("systemfaultword7")]
        public int Systemfaultword7 { get; set; }

        [JsonPropertyName("pdischarge1")]
        public int Pdischarge1 { get; set; }

        [JsonPropertyName("p1charge1")]
        public int P1charge1 { get; set; }

        [JsonPropertyName("vbat")]
        public int Vbat { get; set; }

        [JsonPropertyName("SOC")]
        public int SOC { get; set; }

        [JsonPropertyName("pactouserr")]
        public int Pactouserr { get; set; }

        [JsonPropertyName("pactousertot")]
        public int Pactousertot { get; set; }

        [JsonPropertyName("pactogridr")]
        public int Pactogridr { get; set; }

        [JsonPropertyName("pactogridtot")]
        public int Pactogridtot { get; set; }

        [JsonPropertyName("plocaloadr")]
        public int Plocaloadr { get; set; }

        [JsonPropertyName("plocaloadtot")]
        public int Plocaloadtot { get; set; }

        [JsonPropertyName("spdspstatus")]
        public int Spdspstatus { get; set; }

        [JsonPropertyName("spbusvolt")]
        public int Spbusvolt { get; set; }

        [JsonPropertyName("etouser_tod")]
        public int EtouserTod { get; set; }

        [JsonPropertyName("etouser_tot")]
        public int EtouserTot { get; set; }

        [JsonPropertyName("etogrid_tod")]
        public int EtogridTod { get; set; }

        [JsonPropertyName("etogrid_tot")]
        public int EtogridTot { get; set; }

        [JsonPropertyName("edischarge1_tod")]
        public int Edischarge1Tod { get; set; }

        [JsonPropertyName("edischarge1_tot")]
        public int Edischarge1Tot { get; set; }

        [JsonPropertyName("eharge1_tod")]
        public int Eharge1Tod { get; set; }

        [JsonPropertyName("eharge1_tot")]
        public int Eharge1Tot { get; set; }

        [JsonPropertyName("elocalload_tod")]
        public int ElocalloadTod { get; set; }

        [JsonPropertyName("elocalload_tot")]
        public int ElocalloadTot { get; set; }
    }
}

[JsonSerializable(typeof(GrottState))]
internal partial class GrottContext : JsonSerializerContext
{

}
