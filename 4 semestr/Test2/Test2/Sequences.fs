module Sequences

let infiniteSigns = Seq.initInfinite (fun n -> if n % 2 = 0 then 1 else -1)
let infiniteNumbers = Seq.initInfinite (fun n -> n + 1)

let createSeq () = 
    Seq.map2 (fun x y -> x * y) infiniteSigns infiniteNumbers

printfn "%A" infiniteSigns
printfn "%A" infiniteNumbers
printfn "%A" (createSeq ())