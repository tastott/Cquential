namespace Tim.Cquential.Core
    
    type IMatchCandidate<'T> =
        abstract member Put : 'T -> IMatchCandidate<'T>
        abstract member IsMatch : bool
        abstract member MatchIsStable : bool

