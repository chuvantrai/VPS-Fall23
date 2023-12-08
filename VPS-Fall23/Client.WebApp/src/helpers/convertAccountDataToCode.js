const convertAccountDataToCode = (username, password) => {
  return btoa(JSON.stringify({ username: username, password: password }));
};

export default convertAccountDataToCode;