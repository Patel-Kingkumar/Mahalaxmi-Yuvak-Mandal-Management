export const API_ENDPOINTS = {
  USERS: {
    BASE: 'Users',
    GET_ALL: 'GetAllUsers',
    DOWNLOAD_PDF: 'DownloadUsersPdf'
  },

  AUTH: {
    BASE: 'Auth',
    DOWNLOAD_PDF: 'DownloadAdmnsPdf'
  },

  DONATIONS: {
    BASE: 'Donation',
    CREATE: 'create-donation',
    GET_ALL: 'get-donations',
    GET_BY_ID: 'get-donation',
    UPDATE: 'update-donation',
    DELETE: 'delete-donation',
    DOWNLOAD_PDF: 'DownloadDonationReport'
  },

  MATCHES: {
    BASE: 'Match',
    CREATE: 'create-match',
    GET_ALL: 'get-matches',
    GET_BY_ID: 'get-match',
    UPDATE: 'update-match',
    DELETE: 'delete-match'
  },

  PLAYER_STATS: {
    BASE: 'PlayerStats',
    CREATE: 'CreatePlayerStats',
    GET_ALL: 'GetAllPlayerStats'
  },

  MATCH_SCORE: {
    BASE: 'MatchScore',
    CREATE: 'create-score',
    GET_BY_MATCH: 'get-scores'
  },

  DASHBOARD: {
    BASE: 'dashboard',
    GET_STATS: 'dashboard'
  },
};
