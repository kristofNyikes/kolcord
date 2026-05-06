export const registrationChecker = (
  email: string,
  password: string,
  username: string,
): string[] => {
  const errorList: string[] = [];
  if (!username || username.trim() === "") {
    errorList.push("Username field is required");
  }

  if (!email || email.trim() === "") {
    errorList.push("Email field is required");
  } else if (!email.includes("@")) {
    errorList.push("Email address is invalid");
  }

  if (!password || password.trim() === "") {
    errorList.push("Password is required");
  } else if (password.length < 3 || password.length > 16) {
    errorList.push("Password must be 3-16 characters long");
  }

  if (!/[A-Z]/.test(password)) {
    errorList.push("Password must contain at least one uppercase letter");
  }

  if (!/[0-9]/.test(password)) {
    errorList.push("Password must contain at least one number");
  }

  return errorList;
};

export const loginChecker = (email: string, password: string): string[] => {
  const errorList: string[] = [];
  if (!email || email.trim() === "") {
    errorList.push("Email field is required");
  } else if (!email.includes("@")) {
    errorList.push("Invalid email");
  }

  if (!password || password.trim() === "") {
    errorList.push("Password field is required");
  }
  return errorList;
};
