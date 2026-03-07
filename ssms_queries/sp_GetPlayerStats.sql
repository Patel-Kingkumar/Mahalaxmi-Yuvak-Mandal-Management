CREATE PROCEDURE sp_GetPlayerStats
(
    @UserId INT = NULL
)
AS
BEGIN

SELECT
    ps.PlayerStatId,
    ps.MatchId,
    ps.UserId,
    u.FullName,
    ps.TeamName,
    ps.RunsScored,
    ps.BallsPlayed,
    ps.Fours,
    ps.Sixes,
    ps.WicketsTaken,
    ps.OversBowled,
    ps.RunsGiven,
    ps.Catches
FROM PlayerStats ps
INNER JOIN Users u ON ps.UserId = u.Id
WHERE
    (@UserId IS NULL OR ps.UserId = @UserId)

END
