export type Requests = {
  id: number;
  sender: UserDto;
  friendRequestStatus: number;
  createdAt: Date;
};
export type UserDto = {
  id: string;
  avatar: string;
  bio: string;
  userName: string;
};
export type FriendRequestItemProp = {
  request: Requests;
  removeRequest(): void;
};

export type Data = {
  userName: string;
  email: string;
  token: string;
  refreshToken: string;
  userId: string;
};

export type FriendItemType = {
  friend: UserDto;
};

export type RoundImageProps = {
  src: string;
  size: '8' | '11' | '32';
};

export type InputFieldProp = {
  inputValue: string;
  inputState: (value: string) => void;
  type: string;
  children: React.ReactNode;
};

export type LogoProp = {
  route: string;
};

export type SpinnerProp = {
  size?: number;
  color?: string;
  animationSpeed?: number;
};

export type AuthData = {
  userName: string;
  email: string;
  token: string;
  refreshToken: string;
  userId: string;
};

export type FriendRequestsContextType = {
  friendRequests: Requests[] | null;
  refetchFriendRequests: () => void;
};

export type ProtectedRouteProps = {
  children: React.ReactNode;
};

export type SearchResultItemType = {
  result: UserWithFrStatus;
};

export type SearchButtonProp = {
  onSearch: () => void;
};

export type UserWithFrStatus = {
  id: string;
  avatar: string;
  bio: string;
  userName: string;
  isFriend: boolean;
};

export type SignalREventHandler = (data: Requests) => void;

export type Friend = {
  id: number;
  userId: string;
  friendDto: FriendDto;
};

export type FriendDto = {
  id: string;
  avatar: string;
  bio: string;
  userName: string;
};

export interface MessageDto {
  id: number;
  content: string;
  timeStamp: string;
  senderId: string;
  senderName: string;
  conversationId: number;
}

export interface ConversationDto {
  id: number;
  name: string;
  type: 'Direct' | 'Group' | 'Channel';
  createdAt: string;
  participants: ParticipantDto[];
  lastMessage?: MessageDto;
}

export interface ParticipantDto {
  userId: string;
  userName: string;
}

export interface LoginRegisterProps{
  setModal: React.Dispatch<React.SetStateAction<boolean>>
}