open System.Reflection;;
open System;
open Microsoft.FSharp.Reflection;;

type Stat =
  |Str
  |Dex
  |Con
  |Int
  |Wis
  |Char
  |Crime
  |MaxHP

type EitherStat =
  |OrigStat of Stat
  |NewStat of string  

type stats = {orig: Map<Stat, int>; extra: Map<string, int>}

type rank = {str: int; dex: int; con: int; int: int; wis: int; char: int; hpPerLev: int;} 

type race = {levBonus: int; bnsFreq: int; bnsStat: Stat;}

type creature = {stats: stats; rank: rank; race: race}

let races = [("continental", {levBonus=1; bnsFreq=2; bnsStat=Char});
             ("aryan", {levBonus=1; bnsFreq=2; bnsStat=Str});
             ("arab", {levBonus=1; bnsFreq=2; bnsStat=Con});
             ("jew", {levBonus=1; bnsFreq=2; bnsStat=Int});
             ("asian", {levBonus=1; bnsFreq=2; bnsStat=Dex});
             ("indian", {levBonus=1; bnsFreq=2; bnsStat=Wis});
             ("black", {levBonus=1; bnsFreq=1; bnsStat=Crime});
             ("slav", {levBonus=2; bnsFreq=1; bnsStat=MaxHP})] |>Map.ofList;;

let ranks = [("prole", {str=12; dex=14; con=13; int=9; wis=5; char=7; hpPerLev=12});
             ("lowerClass", {str=14; dex=12; con=11; int=6; wis=9; char=8; hpPerLev=10});
             ("upperClass", {str=5; dex=6; con=12; int=14; wis=12; char=11; hpPerLev=8});
             ("nuveau", {str=10; dex=5; con=9; int=12; wis=9; char=15; hpPerLev=6});
             ("noble", {str=4; dex=7; con=6; int=12; wis=16; char=13; hpPerLev=8});] |>Map.ofList;;

let str2Stat (str: string) =
  match Array.tryFind (fun (case: UnionCaseInfo) -> String.Compare(case.Name, str, true) = 0)
    (FSharpType.GetUnionCases(Str.GetType())) with
      |Some case -> OrigStat ((FSharpValue.MakeUnion (case, null)) :?> Stat)
      |None -> NewStat str
