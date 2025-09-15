import { CommentGet } from "./Comment"

export type PortifolioGet = {
  id: number
  symbol: string
  companyName: string
  purchase: number
  divdend: number
  lastDiv: number
  industry: string
  markCap: number
  comments: CommentGet[]
}

export type PortifolioPost = {
  symbol: string;
}