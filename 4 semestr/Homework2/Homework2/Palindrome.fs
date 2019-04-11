module Palindrome

let isPalindrome (string:string) =
    let reverse (list:list<_>) =
        let rec reverseHelp (list1:list<_>) (list2:list<_>) numberOfElement =
            if numberOfElement = list.Length then list2
            else
                reverseHelp list1.Tail (list1.Head :: list2) (numberOfElement + 1)
                
        match list with
        | [] | [_] -> list
        | _ -> reverseHelp list [] 0

    reverse (Seq.toList string) = (Seq.toList string)