export type Requests = {
  id: number;
  sender: FriendDto;
  friendRequestStatus: number;
  createdAt: Date;
}
export type FriendDto = {
  id: string;
  avatar: string;
  bio: string;
  userName: string;
};
export type FriendRequestItemProp = {
  request: Requests
  removeRequest (): void;
}