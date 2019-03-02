module Palindrome

let isPalindrome (string:string) =
    let reverse (list:list<_>) =
        let rec reverseHelp (list1:list<_>) (list2:list<_>) numberOfElement =
            if numberOfElement = list.Length then list2
            else
                reverseHelp list1.Tail (list1.Head :: list2) (numberOfElement + 1)

        if list.Length = 1 || list.Length = 0 then list
        else reverseHelp list [] 0

    if reverse (Seq.toList string) = (Seq.toList string) then true
    else false