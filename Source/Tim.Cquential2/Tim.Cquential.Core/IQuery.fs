namespace Tim.Cquential.Core

    type IQuery<'T> =
        abstract member GetMatchCandidate : IMatchCandidate<'T>