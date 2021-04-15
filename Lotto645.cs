class Lotto645
{
    public int 회차 { get; set; }
    public System.DateTime 추첨일 { get; set; }

    public int 당첨자수1 { get; set; }
    public decimal 당첨금1 { get; set; }

    public int 당첨자수2 { get; set; }
    public decimal 당첨금2 { get; set; }

    public int 당첨자수3 { get; set; }
    public decimal 당첨금3 { get; set; }

    public int 당첨자수4 { get; set; }
    public decimal 당첨금4 { get; set; }

    public int 당첨자수5 { get; set; }
    public decimal 당첨금5 { get; set; }

    public int[] 당첨번호 { get; set; }
    public int 보너스 { get; set; }

    // public long 인당금액1 => this.당첨금1 / 당첨자수1;
}