const SetToken = (token) => {
    localStorage.setItem("token", token);
};

const GetToken = () => {
    return localStorage.getItem("token");
};

const HasToken = () => {
    return !!localStorage.getItem("token");
};

export { SetToken, GetToken, HasToken };