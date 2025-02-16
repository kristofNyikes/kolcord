export type Requests = {
  id: number;
  sender: UserDto;
  friendRequestStatus: number;
  createdAt: Date;
}
export type UserDto = {
  id: string;
  avatar: string;
  bio: string;
  userName: string;
};
export type FriendRequestItemProp = {
  request: Requests
  removeRequest (): void;
}

export type Data = {
  userName: string;
  email: string;
  token: string;
  refreshToken: string;
};

export type FriendItemType = {
  friend: UserDto
}

export type RoundImageProps = {
  src: string
  size: '8' | '11' | '32'
}

export type InputFieldProp = {
  inputValue : string;
  inputState: (value: string) => void;
  type: string
  children: React.ReactNode;
}

export type LogoProp = {
  route: string;
}

export type SpinnerProp = {
  size?: number;
  color?: string;
  animationSpeed?: number;
};

export type AuthData = {
  userName: string;
  email: string;
  accessToken: string;
  refreshToken: string;
};

export type FriendRequestsContextType = {
  friendRequests: Requests[] | null;
  refetchFriendRequests: () => void;
};

export type ProtectedRouteProps = {
  children: React.ReactNode;
}

export type SearchResultItemType = {
  result: UserWithFrStatus;
}

export type SearchButtonProp = {
  onSearch: () => void;
}

export type UserWithFrStatus = {
  id: string;
  avatar: string;
  bio: string;
  userName: string;
  isFriend: boolean;
}